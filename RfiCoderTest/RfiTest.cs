/*
 * Created by SharpDevelop.
 * User: brian
 * Date: 5/14/2014
 * Time: 6:23 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;

namespace RfiCoderTest
{
  [TestFixture]
  public class RfiTest
  {
    [Test]
    public void TestCategorization1()
    {
      var subjectLine = "Submitted RFI# 9 for store# 7225 dated 2014-05-10";
      
      string categoryResult;
      
      RfiCoder.Entity.Store store;
      
      var result = RfiCoder.Utilities.Categorizer.InstanceOf.TryCategorizationFromString(subjectLine, out categoryResult, out store);
      
      Assert.That(result, Is.EqualTo(RfiCoder.Enum.QuestionTypes.Success));
      
      Assert.That(categoryResult, Is.EqualTo("Williamsburg 7225-0 RFI: 009"));
    }
    
    [Test]
    public void TestCategorization2()
    {
      var subjectLine = "RFI:Request for Information Submitted: 4994-001 (Mechanicsburg, PA) RFI: 28";
      
      string categoryResult;
      
      RfiCoder.Entity.Store store;
      
      var result = RfiCoder.Utilities.Categorizer.InstanceOf.TryCategorizationFromString(subjectLine, out categoryResult, out store);
      
      Assert.That(result, Is.EqualTo(RfiCoder.Enum.QuestionTypes.Success));
      
      Assert.That(categoryResult, Is.EqualTo("Mechanicsburg 4994-1 RFI: 028"));
    }
    
    [Test]
    public void TestCategorization3()
    {
      var subjectLine = "RFI:Request for Information Submitted: 4994 (Mechanicsburg, PA) RFI: 28";
      
      string categoryResult;
      
      RfiCoder.Entity.Store store;
      
      var result = RfiCoder.Utilities.Categorizer.InstanceOf.TryCategorizationFromString(subjectLine, out categoryResult, out store);
      
      Assert.That(result, Is.EqualTo(RfiCoder.Enum.QuestionTypes.Success));
      
      Assert.That(categoryResult, Is.EqualTo("Mechanicsburg 4994-1 RFI: 028"));
    }
    
    [Test]
    public void TestCategorization4()
    {
      var subjectLine = "WM Store #3512 Sink Drain Dimensions RFI";
      
      string categoryResult;
      
      RfiCoder.Entity.Store store;
      
      var result = RfiCoder.Utilities.Categorizer.InstanceOf.TryCategorizationFromString(subjectLine, out categoryResult, out store);
      
      Assert.That(result, Is.EqualTo(RfiCoder.Enum.QuestionTypes.RequestForInformation));
      
      Assert.That(categoryResult, Is.EqualTo("Albuquerque (montgomery) 3512-0 RFI"));
    }
    
    [Test]
    public void TestCategorization5()
    {
      var subjectLine = "RE: RFI:Request for Information Submitted 4799-200 (Citrus Heights, CA) - RM RFI 10 ";
      
      string categoryResult;
      
      RfiCoder.Entity.Store store;
      
      var result = RfiCoder.Utilities.Categorizer.InstanceOf.TryCategorizationFromString(subjectLine, out categoryResult, out store);
      
      Assert.That(result, Is.EqualTo(RfiCoder.Enum.QuestionTypes.Success));
      
      Assert.That(categoryResult, Is.EqualTo("Citrus Heights 4799-200 RFI: 010"));
    }
    
    [Test]
    public void TestOverDue()
    {
      
    }
    
    [Test]
    public void TestRfiResponse()
    {
      var email = new Microsoft.Exchange.WebServices.Data.EmailMessage(
        RfiCoder.Utilities.EmailHelper.InstanceOf.GetEmailService);
      
      email.Subject = "RFI:Request for Information Submitted: 4994-001 (Mechanicsburg, PA) RFI: 28";
      
      RfiCoder.Entity.Store store;
      
      string category;
      
      bool isSuccess = false;
      
      var emailSubject = email.Subject;
      
      var result = RfiCoder.Utilities.Categorizer.InstanceOf.TryCategorizationFromString(emailSubject, out category, out store);
      
      if (result == RfiCoder.Enum.QuestionTypes.Success) {
        email.Categories.Clear();
        
        email.Categories.Add(category);
        
        isSuccess = true;
      } else if (result == RfiCoder.Enum.QuestionTypes.RequestForInformation) {
        result = RfiCoder.Utilities.Categorizer.InstanceOf.TryCategorizationFromText(email.Body, store, out category);
        
        if (result == RfiCoder.Enum.QuestionTypes.Success) {
          email.Categories.Clear();
          
          email.Categories.Add(category);
          
          isSuccess = true;
        }
      }
      
      Assert.That(isSuccess, Is.True);
      
      Assert.That(category, Is.EqualTo("Mechanicsburg 4994-1 RFI: 028"));
    }
    
    [Test]
    public void TestRfiCategory()
    {
      var email = new Microsoft.Exchange.WebServices.Data.EmailMessage(
        RfiCoder.Utilities.EmailHelper.InstanceOf.GetEmailService);
      
      email.Subject = "RFI:Request for Information Submitted: 4994-001 (Mechanicsburg, PA) RFI: 28";
      
      email.Body = "RFI:Request for Information Submitted: 4994-001 (Mechanicsburg, PA) RFI: 28";
      
      RfiCoder.Entity.Store store;
      
      var isSuccess = RfiCoder.Utilities.EmailHelper.InstanceOf.AssignCategory(email, out store);
      
      Assert.That(isSuccess, Is.True);
      
      Assert.That(email.Categories[0], Is.EqualTo("Mechanicsburg 4994-1 RFI: 028"));
    }
    
    [Test]
    public void TestRfiResponse2()
    {
      var email = new Microsoft.Exchange.WebServices.Data.EmailMessage(
        RfiCoder.Utilities.EmailHelper.InstanceOf.GetEmailService);
      
      email.Subject = @"FW: # 2345-00 (Italy, TX) - Request for Information";
      
      email.Body = @"


________________________________________
From: EDDS | Request For Information
Sent: Wednesday, May 28, 2014 10:32:04 AM (UTC-06:00) Central Time (US & Canada)
To: Amy Mills
Subject: # 2345-00 (Italy, TX) - Request for Information

REQUEST FOR INFORMATION:

DATE: 5/28/2014

TO: Amy Mills
    Raymond Harris and Associates

FROM: Matt Scott (Haines, Jones & Cadbury)
     [matt.scott@hjcinc.com]  (479) 756-8989

RFI NUMBER: v2345-00-01

STORE: 2345-00 (Italy, TX)

SPECIFICATION SECTION:

DRAWING/DETAIL:
        0010 - CS1.0 COVER SHEET - 2345
        8010 - FSA1 CANOPY AND DETAILS - 2345

USER ATTACHMENT:


INFORMATION REQUESTED:

Please advise if there will be any plumbing pages associated with the fuel station?

Matt Scott
479-756-8989 Ext 5958


Use the following link to reply to this request:

http://www.bldgportal.com/login.aspx?appId=EDDS&ID=129564

-- Please do not reply to this message. This mailbox is unattended. --
";
      
      RfiCoder.Entity.Store store;
      
      var isSuccess = RfiCoder.Utilities.EmailHelper.InstanceOf.AssignCategory(email, out store);
      
      Assert.That(isSuccess, Is.True);
      
      Assert.That(email.Categories[0], Is.EqualTo("Italy 2345-0 RFI: v2345-00-01"));
    }
    
    [Test]
    public void TestRfiResponse3()
    {
      var email = new Microsoft.Exchange.WebServices.Data.EmailMessage(
        RfiCoder.Utilities.EmailHelper.InstanceOf.GetEmailService);
      
      email.Subject = @"Fwd: Re: Fwd: FW: RFI:Request for Information Submitted: 3517-000 (Rio Rancho (";
      
      email.Body = @"
Please see attached response to the RFI listed above.

Thank you,

Mitzi Stone
Wallace Engineering Structural Consultants, Inc.
Structural and Civil Consultants
200 East Mathew Brady Street
Tulsa, Oklahoma 74103
918.584.5858
www.wallacesc.com

This email and any files transmitted with it may contain confidential or privileged information.  If you have received this email message in error, please notify the sender by email and delete this email from your system.  The unauthorized use or dissemination of confidential or privileged information contained in this email is prohibited.

----- Original Message -----
        
From:           Max Lehman      Wednesday, May 28, 2014 2:15:37 PM
Subject:        Re: Fwd: FW: RFI:Request for Information Submitted: 3517-000 (Rio Rancho (South
To:             WM RFI's

Response:
The specified conditions with one bar per cell will be acceptable.  Continue constructing walls with 2 bars per cell at the required locations per the documents.  The scuppers are the openings that are the most critical in placing the reinforcement in the correct locations.  Do not build scupper jambs with only a single reinforcing bar per cell higher than the 4' already indicated or wall will need to be opened up to provide the correct amount of reinforcement.

Contractor Error
CMU
45min
incorrectly placed jamb reinforcement

Thanks.

Max Lehman, PE
Associate

Wallace Engineering - Structural Consultants, Inc.
Structural and Civil Consultants
900 West Castleton Road, Suite 140
Castle Rock, Colorado 80109
720.407.5289 Direct
303.350.1690 Office
www.wallacesc.com

Tulsa | Kansas City | Oklahoma City | Denver | Atlanta

Connect with us:  Facebook | LinkedIn | Twitter | Origin

This email and any files transmitted with it may contain confidential or privileged information.  If you have received this email message in error, please notify the sender by email and delete this email from your system.  The unauthorized use or dissemination of confidential or privileged information contained in this email is prohibited.

WM RFI's on Wednesday, May 28, 2014 at 8:43 AM -0600 wrote:

----- Original Message -----
        
From:           RFI <RFI@rhaaia.com>  Wednesday, May 28, 2014 9:45:04 AM
Subject:        FW: RFI:Request for Information Submitted: 3517-000 (Rio Rancho (Southern), NM) R
To:             WM RFI's
Cc:             RFI <RFI@rhaaia.com>  Alyssa Parker <AParker@rhaaia.com>

Please see request for information below and respond as soon as possible.

PLEASE DO NOT MODIFY SUBJECT LINE

Jim Sims
211 N. Record Street
Suite 222
Dallas, TX 75202
(214) 749-0626


-----Original Message-----
From: Amy Mills
Sent: Tuesday, May 27, 2014 4:07 PM
To: RFI
Subject: FW: RFI:Request for Information Submitted: 3517-000 (Rio Rancho (Southern), NM) RFI: 14



________________________________________
From: notifications@evoco.com
Sent: Tuesday, May 27, 2014 4:02:58 PM (UTC-06:00) Central Time (US & Canada)
To: Amy Mills
Cc: Gilbert Jordan; Jim Sims; Barbara Forster; Joey Hiers; Nathan Lantz; Nicholas Fingerhut; Pej Haidari; Vince Herrera
Subject: RFI:Request for Information Submitted: 3517-000 (Rio Rancho (Southern), NM) RFI: 14

Project: 3517-000 (Rio Rancho (Southern), NM) RFI: 14


The following RFI has been submitted and requires a response within 3 days.

http://www.bldgportal.com/?appId=RFI&RID=193043

RFI: 14

Project: 3517-000

Submitted: Tuesday, May 27, 2014

Submitted by: Nathan Lantz, Roche Constructors Inc

Assigned to: Amy Mills, Raymond Harris and Associates


Information requested: Please reference detail #2 on sheet S4.  Per the detail it shows and has a note that the there needs to be two bars in the jambs of a typical masonry wall opening.  The issue is that we only installed one bar in the jambs, the number of cells on each jamb is accurate, and we are just missing the second bar in each cell.  See attached for locations of issues.  Please advise if the single bar in the jambs is adequate, if not please provide instructions and details for proper fix.

1)      Door 121A - Missing second bar at jambs.  Wall is currently at 12'-0";
      
      RfiCoder.Entity.Store store;
      
      var isSuccess = RfiCoder.Utilities.EmailHelper.InstanceOf.AssignCategory(email, out store);
      
      Assert.That(isSuccess, Is.True);
      
      Assert.That(email.Categories[0], Is.EqualTo("Rio Rancho (southern) 3517-0 RFI: 014"));
    }
    
    [Test]
    public void TestRfiResponse4()
    {
      var email = new Microsoft.Exchange.WebServices.Data.EmailMessage(
        RfiCoder.Utilities.EmailHelper.InstanceOf.GetEmailService);
      
      email.Subject = @"WM Store #3512 Sink Drain Dimensions RFI";
      
      email.Body = @"> From: Jim Rich
> Sent: Tuesday, June 10, 2014 12:31 PM
> To: pesebio@rhaaia.com<mailto:pesebio@rhaaia.com>
> Cc: leonardgabaldon@mickrichcontractors.com<mailto:leonardgabaldon@mickrichcontractors.com>
> Subject: WM Store #3512 Sink Drain Dimensions RFI
>
> Polo
>
> My plumber has asked that I get the floor drain dimensions on the attached drawing. I have clouded the drains we need dimensions on away from grid line #6. We have looked at all drawings and compared it with the submittals provided by the supplier and we cannot find a hard dimension. If you could let me know I would appreciate it, or direct me to where they might be.
>
> Thanks
>
>
> Jim Rich
> Project Manager/Superintendent
> Mick Rich Contractors, Inc.
> O: 505.823.9782 Ext. 15
> M: 505.353.2372
> F: 505.823.9783
>
> [MickRichlogo_tagline copy2]
>
>
> <image001.jpg>
> <Mechanical-Plumbing 16.pdf>
";
      
      RfiCoder.Entity.Store store;
      
      var isSuccess = RfiCoder.Utilities.EmailHelper.InstanceOf.AssignCategory(email, out store);
      
      Assert.That(isSuccess, Is.True);
      
      Assert.That(email.Categories[0], Is.EqualTo("Albuquerque (montgomery) 3512-0 RFI"));
    }
    
    [Test]
    public void TestRfiResponse5()
    {
      var email = new Microsoft.Exchange.WebServices.Data.EmailMessage(
        RfiCoder.Utilities.EmailHelper.InstanceOf.GetEmailService);
      
      email.Subject = @"FW: # 7310-00 (Cinco Ranch, TX) - Request for Information";
      
      email.Body = @"

REQUEST FOR INFORMATION:

DATE: 12/26/2014

TO: Amy Mills
    Raymond Harris and Associates

FROM: Tina Ames (Wal-Mart Stores Inc.)
     [tina.ames@wal-mart.com]  (479) 204-0517

RFI NUMBER: v7310-00-08

STORE: 7310-00 (Cinco Ranch, TX)

SPECIFICATION SECTION: 

DRAWING/DETAIL: 
	FX1-FIXTURE-GONDALAPLAN_12-09-14

USER ATTACHMENT:


INFORMATION REQUESTED:

I am need a pdf drawing of the FXS1 to get the information for the bolts and anchors so I can order them

";
      
      RfiCoder.Entity.Store store;
      
      var isSuccess = RfiCoder.Utilities.EmailHelper.InstanceOf.AssignCategory(email, out store);
      
      Assert.That(isSuccess, Is.True);
      
      Assert.That(email.Categories[0], Is.EqualTo("Cinco Ranch (westheimer) 7310-0 RFI: v7310-00-08"));
    }
    
    [Test]
    public void TestRfiResponse6()
    {
      var email = new Microsoft.Exchange.WebServices.Data.EmailMessage(
        RfiCoder.Utilities.EmailHelper.InstanceOf.GetEmailService);
      
      email.Subject = @"FW: Borger, TX - #1516 - RFI #64";
      
      email.Body = @"

Project: 1516-002 (Borger, TX) RFI: 64



The following RFI has been submitted and requires a response within 3 days.

http://www.bldgportal.com/?appId=RFI&RID=205859

RFI: 64

Project: 1516-002

Submitted: Friday, November 07, 2014

Submitted by: John Wilson, M W BUILDERS INC.

Assigned to: Terri Hicks, SGA Design Group



Information requested: in the fuel station we installed per the plans, to the left of the door that enters into the equipment room, a light switch, the alarm panel and the thermostat.  there are racks that go against this wall that will not fit because of the devices installed.  it is at the request of the walmart cm to move the light switch directly on the other side of the wall from where it is at, into the equipment room, and to move the thermostat and fire alarm panel to the wall in between the equipment room door and bathroom door, just above the knee wall.  is this acceptable?

0
Attached is a .pdf of an RFI for the above mentioned project.



";
      
      RfiCoder.Entity.Store store;
      
      var isSuccess = RfiCoder.Utilities.EmailHelper.InstanceOf.AssignCategory(email, out store);
      
      Assert.That(isSuccess, Is.True);
      
      Assert.That(email.Categories[0], Is.EqualTo("Rio Rancho (southern) 3517-0 RFI: 014"));
    }
    
    [Test]
    public void TestRfiResponseWithOutSequence()
    {
      var email = new Microsoft.Exchange.WebServices.Data.EmailMessage(
        RfiCoder.Utilities.EmailHelper.InstanceOf.GetEmailService);
      
      email.Subject = "RFI:Request for Information Submitted: 1536 (Mechanicsburg, PA) RFI: 28";
      
      string category;
      
      RfiCoder.Entity.Store store;
      
      var emailSubject = email.Subject;
      
      var result = RfiCoder.Utilities.Categorizer.InstanceOf.TryCategorizationFromString(emailSubject, out category, out store);
      
      Assert.That(result, Is.EqualTo(RfiCoder.Enum.QuestionTypes.Success));
      
      Assert.That(category, Is.EqualTo("Seminole 1536-5 RFI: 028"));
    }
    
    [Test]
    public void TestNonRfiResponseWithOutSequence()
    {
      var email = new Microsoft.Exchange.WebServices.Data.EmailMessage(
        RfiCoder.Utilities.EmailHelper.InstanceOf.GetEmailService);
      
      email.Subject = "Submitted: 1536 Seminole, FL - wsss dated 2014-12-12";
      
      string category;
      
      RfiCoder.Entity.Store store;
      
      var emailSubject = email.Subject;
      
      var result = RfiCoder.Utilities.Categorizer.InstanceOf.TryCategorizationFromString(emailSubject, out category, out store);
      
      Assert.That(result, Is.EqualTo(RfiCoder.Enum.QuestionTypes.None));
    }
    
    [Test]
    public void TestTryGetAndAttachDocuments ()
    {
      var email = new Microsoft.Exchange.WebServices.Data.EmailMessage(
        RfiCoder.Utilities.EmailHelper.InstanceOf.GetEmailService);
      
      var store = new RfiCoder.Entity.Store();
      
      store.Number = 4162;
      
      store.Sequence = 0;
      
      var category = "RFI: 109";
      
      var result = RfiCoder.Utilities.EmailHelper.InstanceOf.TryGetAndAttachDocuments(email, store, category);
      
      Assert.That(result, Is.True);
      
      var attachments = email.Attachments;
      
      Assert.That( attachments.Count, Is.InRange(1,6) );
    }
    
    [Test]
    public void TestTryGetAndAttachDocuments2 ()
    {
      var email = new Microsoft.Exchange.WebServices.Data.EmailMessage(
        RfiCoder.Utilities.EmailHelper.InstanceOf.GetEmailService);
      
      var store = new RfiCoder.Entity.Store();
      
      store.Number = 6406;
      
      store.Sequence = 3;
      
      var category = "RFI: 072";
      
      var result = RfiCoder.Utilities.EmailHelper.InstanceOf.TryGetAndAttachDocuments(email, store, category);
      
      Assert.That(result, Is.True);
      
      var attachments = email.Attachments;
      
      Assert.That( attachments.Count, Is.InRange(1,6) );
      
      int count = 1;
      
      var parser = new RfiCoder.Utilities.Parser();
      
      foreach (var attachment in attachments) {
        var name = attachment.Name;
        
        var extension = parser.GetAttachmentExtension(name);
        
        var rfiNumber = parser.GetRfiNumberWithPadding(category);
        
        var rename = String.Format( "{0}_RFI {1}_Attachment {2}{3}", store.Number.ToString(), rfiNumber, count.ToString("D2"), extension);
        
        Assert.That( name, Is.EqualTo(rename) );
        
        count ++;
      }
    }
    
    [Test]
    public void TestTryGetAndAttachDocumentsFail ()
    {
      var email = new Microsoft.Exchange.WebServices.Data.EmailMessage(
        RfiCoder.Utilities.EmailHelper.InstanceOf.GetEmailService);
      
      var store = new RfiCoder.Entity.Store();
      
      store.Number = 4114;
      
      store.Sequence = 0;
      
      var category = "RFI: 1";
      
      var result = RfiCoder.Utilities.EmailHelper.InstanceOf.TryGetAndAttachDocuments(email, store, category);
      
      Assert.That(result, Is.False);
      
      var attachments = email.Attachments;
      
      Assert.That( attachments.Count, Is.EqualTo(0) );
    }
    
    [Test]
    public void TestTryGetAndAttachDocumentsMultiStore ()
    {
      var email = new Microsoft.Exchange.WebServices.Data.EmailMessage(
        RfiCoder.Utilities.EmailHelper.InstanceOf.GetEmailService);
      
      var store = new RfiCoder.Entity.Store();
      
      store.Number = 6589;
      
      store.Sequence = 0;
      
      var category = "RFI: 1";
      
      var result = RfiCoder.Utilities.EmailHelper.InstanceOf.TryGetAndAttachDocuments(email, store, category);
      
      Assert.That(result, Is.False);
      
      var attachments = email.Attachments;
      
      Assert.That( attachments.Count, Is.EqualTo(0) );
    }
    
    [Test]
    public void TestGetProjectFolder ()
    {
      var store = new RfiCoder.Entity.Store();
      
      store.Program = RfiCoder.Enum.ProgramTypes.NewStores;
      
      store.ProjectNumber = 1404016;
      
      var result = RfiCoder.Utilities.EmailHelper.InstanceOf.GetOutlookProjectPath(store);
      
      var name = Microsoft.Exchange.WebServices.Data.Folder.Bind(RfiCoder.Utilities.EmailHelper.InstanceOf.GetEmailService, result).DisplayName;
      
      Assert.That( name, Is.EqualTo("Broomfield, CO #85527 (1404016)") );
    }
    
    [Test]
    public void TestGetProjectFolder2 ()
    {
      var store = new RfiCoder.Entity.Store();
      
      store.Program = RfiCoder.Enum.ProgramTypes.Expansions;
      
      store.ProjectNumber = 1201001;
      
      var result = RfiCoder.Utilities.EmailHelper.InstanceOf.GetOutlookProjectPath(store);
      
      var name = Microsoft.Exchange.WebServices.Data.Folder.Bind(RfiCoder.Utilities.EmailHelper.InstanceOf.GetEmailService, result).DisplayName;
      
      Assert.That( name, Is.EqualTo("Paramount, CA #2110 (1201001)") );
    }
    
    [Test]
    public void TestGetProjectRfiFolder ()
    {
      var store = new RfiCoder.Entity.Store();
      
      store.Program = RfiCoder.Enum.ProgramTypes.Express;
      
      store.ProjectNumber = 1417001;
      
      var folderId = RfiCoder.Utilities.EmailHelper.InstanceOf.GetOutlookProjectPath(store);
      
      var result = RfiCoder.Utilities.EmailHelper.InstanceOf.GetProjectRfiFolder(folderId);
      
      var name = Microsoft.Exchange.WebServices.Data.Folder.Bind(RfiCoder.Utilities.EmailHelper.InstanceOf.GetEmailService, result).DisplayName;
      
      Assert.That( name, Is.EqualTo("RFI") );
    }
    
    [Test]
    public void TestGetProjectRfiFolder2 ()
    {
      var store = new RfiCoder.Entity.Store();
      
      store.Program = RfiCoder.Enum.ProgramTypes.NewStores;
      
      store.ProjectNumber = 1305015;
      
      var folderId = RfiCoder.Utilities.EmailHelper.InstanceOf.GetOutlookProjectPath(store);
      
      var result = RfiCoder.Utilities.EmailHelper.InstanceOf.GetProjectRfiFolder(folderId);
      
      var name = Microsoft.Exchange.WebServices.Data.Folder.Bind(RfiCoder.Utilities.EmailHelper.InstanceOf.GetEmailService, result).DisplayName;
      
      Assert.That( name, Is.EqualTo("RFI") );
    }
    
    [Test]
    public void TestGetMessagesOfSameCategory()
    {
      var view = new Microsoft.Exchange.WebServices.Data.ItemView(int.MaxValue);
      
      var items = RfiCoder.Utilities.EmailHelper.InstanceOf.GetEmailService.FindItems(RfiCoder.Utilities.EmailHelper.InstanceOf.NewStoresFolder, view);
      
      var randGen = new System.Random();
      
      var pt = randGen.Next(1, items.TotalCount);
      
      var item = items.Items[pt];
      
      var message = Microsoft.Exchange.WebServices.Data.EmailMessage.Bind(RfiCoder.Utilities.EmailHelper.InstanceOf.GetEmailService,
                                        item.Id,
                                        RfiCoder.Utilities.EmailHelper.EmailProperties
                                       );
      
      var results = RfiCoder.Utilities.EmailHelper.InstanceOf.GetMessagesOfSameCategory(message);
      
      foreach (var element in results.Items) {
        Assert.That( message.Categories[0], Is.EqualTo(element.Categories[0]) );
      }
    }
  }
  
}