/*
 * Created by SharpDevelop.
 * User: bcrawford
 * Date: 7/28/2014
 * Time: 10:53 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using RfiCoder.Utilities;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace RfiCoderTest
{
  /// <summary>
  /// Description of BayesianTest.
  /// </summary>
  [TestFixture]
  public class BayesianTest
  {
    private Microsoft.Exchange.WebServices.Data.EmailMessage email;
    
    [SetUp]
    public void Init()
    {
      var email = new Microsoft.Exchange.WebServices.Data.EmailMessage(
        RfiCoder.Utilities.EmailHelper.InstanceOf.GetEmailService);
    }
    
    [Test]
    public void TestIsRfi()
    {
      this.email = new Microsoft.Exchange.WebServices.Data.EmailMessage(
        RfiCoder.Utilities.EmailHelper.InstanceOf.GetEmailService);
      
      this.email.Subject = @"FW: # 2345-00 (Italy, TX) - Request for Information";
      
      this.email.Body = @"REQUEST FOR INFORMATION:

RFI NUMBER: 

STORE: 

SPECIFICATION SECTION:

DRAWING/DETAIL:
        0010 - CS1.0 COVER SHEET - 2345
        8010 - FSA1 CANOPY AND DETAILS - 2345

USER ATTACHMENT:


INFORMATION REQUESTED:

Please advise if there will be any plumbing pages associated with the fuel station?
";
      
      var result = RfiCoder.Utilities.EmailHelper.InstanceOf.IsRfi(this.email);
      
      Assert.That(result, Is.True);
    }
  }
}
