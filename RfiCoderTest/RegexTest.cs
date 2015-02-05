/*
 * Created by SharpDevelop.
 * User: bcrawford
 * Date: 5/16/2014
 * Time: 7:54 AM
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
  /// RegexTest.
  /// </summary>
  /// 
  [TestFixture]
  public class RegexTest
  {
    private Parser parser;
    
    [SetUp]
    public void Init()
    {
      parser = new Parser();
    }
    
    [Test]
    public void TestGetStoreNumberAndSequenceFromHaystack1()
    {
      string needle = @"1234-123";
      
      var result = parser.GetStoreNumberAndSequenceFromHaystack(needle);
      
      Assert.That(result.Count, Is.EqualTo(2));
      
      Assert.That( result["number"], Is.EqualTo(1234) );
      
      Assert.That( result["sequence"], Is.EqualTo(123) );
    }
    
    [Test]
    public void TestGetStoreNumberAndSequenceFromHaystack2()
    {
      string needle = @"Lorem 1234-123 Ipsum";
      
      var result = parser.GetStoreNumberAndSequenceFromHaystack(needle);
      
      Assert.That( result["number"], Is.EqualTo(1234) );
      
      Assert.That( result["sequence"], Is.EqualTo(123) );
    }
    
    [Test]
    public void TestGetStoreNumberAndSequenceFromHaystack3()
    {
      string needle = @"A Store#1234-123 with other 456";
      
      var result = parser.GetStoreNumberAndSequenceFromHaystack(needle);
      
      Assert.That( result["number"], Is.EqualTo(1234) );
      
      Assert.That( result["sequence"], Is.EqualTo(123) );
    }
    
    [Test]
    public void TestGetStoreNumberAndSequenceFromHaystack4()
    {
      string needle = @"A Store# 1234-123 with other 456-789-1012";
      
      var result = parser.GetStoreNumberAndSequenceFromHaystack(needle);
      
      Assert.That( result["number"], Is.EqualTo(1234) );
      
      Assert.That( result["sequence"], Is.EqualTo(123) );
    }
    
    [Test]
    public void TestGetStoreNumberAndSequenceFromHaystack5()
    {
      string needle = @"Submitted RFI# 9 for store# 7225 dated 2014-05-10";
      
      var result = parser.GetStoreNumberAndSequenceFromHaystack(needle);
      
      Assert.That( result["number"], Is.EqualTo(7225) );
      
      Assert.That( result["sequence"], Is.Null );
    }
    
    [Test]
    public void TestGetStoreNumberAndSequenceFromHaystack6()
    {
      string needle = @"Submitted RFI# 9 for store# 7225 dated 2014/05/10";
      
      var result = parser.GetStoreNumberAndSequenceFromHaystack(needle);
      
      Assert.That( result["number"], Is.EqualTo(7225) );
      
      Assert.That( result["sequence"], Is.Null );
    }
    
    [Test]
    public void TestGetStoreNumberAndSequenceFromHaystack7()
    {
      string needle = @"RFI:Request for Information Submitted: 4994-001 (Mechanicsburg, PA) RFI: 28";
      
      var result = parser.GetStoreNumberAndSequenceFromHaystack(needle);
      
      Assert.That( result["number"], Is.EqualTo(4994) );
      
      Assert.That( result["sequence"], Is.EqualTo(1) );
    }
    
    [Test]
    public void TestGetStoreNumberAndSequenceFromHaystack8()
    {
      string needle = @"FW: # 2345-00 (Italy, TX) - Request for Information";
      
      var result = parser.GetStoreNumberAndSequenceFromHaystack(needle);
      
      Assert.That( result["number"], Is.EqualTo(2345) );
      
      Assert.That( result["sequence"], Is.EqualTo(000) );
    }
    
    [Test]
    public void TestGetStoreNumberAndSequenceFromHaystack10()
    {
      string needle = @"FW: # 3043-02 (Ft. Worth (Beach), TX) - Request for Information";
      
      var result = parser.GetStoreNumberAndSequenceFromHaystack(needle);
      
      Assert.That( result["number"], Is.EqualTo(3043) );
      
      Assert.That( result["sequence"], Is.EqualTo(002) );
    }
    
    [Test]
    public void TestGetStoreNumberAndSequenceFromHaystack9()
    {
      string needle = @"WM Store #3512 Sink Drain Dimensions RFI";
      
      var result = parser.GetStoreNumberAndSequenceFromHaystack(needle);
      
      Assert.That( result["number"], Is.EqualTo(3512) );
      
      Assert.That( result["sequence"], Is.Null );
    }
    
    [Test]
    public void TestGetRfiNumberWithPadding()
    {
      string needle = @"Store# 34 Rfi#12";
      
      var result = parser.GetRfiNumberWithPadding(needle);
      
      Assert.That( result, Is.EqualTo("012") );
    }
    
    [Test]
    public void TestGetRfiNumberWithPadding1()
    {
      string needle = @"Store# 34 Rfi: 12 10";
      
      var result = parser.GetRfiNumberWithPadding(needle);
      
      Assert.That( result, Is.EqualTo("012") );
    }
    
    [Test]
    public void TestNegativeGetRfiNumberWithPadding()
    {
      string needle = @"Store#24 Rf#12 #34 44";
      
      var result = parser.GetRfiNumberWithPadding(needle);
      
      Assert.That( result, Is.Not.EqualTo("012") );
    }
    
    [Test]
    public void TestGetRfiNumberWithPaddingVendor()
    {
      string needle = @"Store#24 Rfi number: v12 #34 44";
      
      var result = parser.GetRfiNumberWithPadding(needle);
      
      Assert.That( result, Is.EqualTo("v12") );
    }
    
    [Test]
    public void TestGetRfiNumberWithPaddingVendor2()
    {
      string needle = @"Store#24  Rfi  number:  v12 #34 44";
      
      var result = parser.GetRfiNumberWithPadding(needle);
      
      Assert.That( result, Is.EqualTo("v12") );
    }
    
    [Test]
    public void TestNegativeGetRfiNumberWithPaddingVendor()
    {
      string needle = @"Store#24  Rfi  number:  v12b #34 44";
      
      var result = parser.GetRfiNumberWithPadding(needle);
      
      Assert.That( result, Is.Not.EqualTo("012") );
    }
    
    [Test]
    public void TestIsBidQuestion()
    {
      string needle = @"Store#24  Rfi  number:  v12b #34 44
        with bid question";
      
      var result = parser.IsBidQuestion(needle);
      
      Assert.That( result, Is.True);
    }
    
    [Test]
    public void TestIsReverseRfi()
    {
      string needle = @"Store#24  reverse Rfi  number:  v12b #34 44
        with bid question";
      
      var result = parser.IsReverseRfi(needle);
      
      Assert.That( result, Is.True);
    }
    
    [Test]
    public void TestIsRfi()
    {
      string needle = @"WM Store #3512 Sink Drain Dimensions RFI";
      
      var result = parser.IsRfiQuestion(needle);
      
      Assert.That( result, Is.True );
    }
    
    [Test]
    public void TestIsRfi2()
    {
      string needle = @"
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
      
      var result = parser.IsRfiQuestion(needle);
      
      Assert.That( result, Is.True );
    }
    
    [Test]
    public void TestGetTimeInterval()
    {
      var someIntervals = new System.Collections.Generic.List< string > {
        "10 Seconds",
        "11 Minutes",
        "12 Hours",
        "13 Days",
        "14 Weeks",
        "15 Months"
      };
      
      var testIntervals = new System.Collections.Generic.List< string > {
        "Seconds",
        "Minutes",
        "Hours",
        "Days",
        "Weeks",
        "Months"
      };
      
      var parser = new RfiCoder.Utilities.Parser();
      
      for (int i = 0; i < 6; i++) {
        var result = parser.GetTimeInterval(someIntervals[i]);
        
        switch (testIntervals[i]) {
          case "Seconds":
            Assert.That(result.ContainsKey(RfiCoder.Enum.TimeInterval.Seconds), Is.True);
            
            Assert.That(result[RfiCoder.Enum.TimeInterval.Seconds], Is.EqualTo(10));
            
            break;
          case "Minutes":
            Assert.That(result.ContainsKey(RfiCoder.Enum.TimeInterval.Minutes), Is.True);
            
            Assert.That(result[RfiCoder.Enum.TimeInterval.Minutes], Is.EqualTo(11));
            
            break;
          case "Hours":
            Assert.That(result.ContainsKey(RfiCoder.Enum.TimeInterval.Hours), Is.True);
            
            Assert.That(result[RfiCoder.Enum.TimeInterval.Hours], Is.EqualTo(12));
            
            break;
          case "Days":
            Assert.That(result.ContainsKey(RfiCoder.Enum.TimeInterval.Days), Is.True);
            
            Assert.That(result[RfiCoder.Enum.TimeInterval.Days], Is.EqualTo(13));
            
            break;
          case "Weeks":
            Assert.That(result.ContainsKey(RfiCoder.Enum.TimeInterval.Weeks), Is.True);
            
            Assert.That(result[RfiCoder.Enum.TimeInterval.Weeks], Is.EqualTo(14));
            
            break;
          case "Months":
            Assert.That(result.ContainsKey(RfiCoder.Enum.TimeInterval.Months), Is.True);
            
            Assert.That(result[RfiCoder.Enum.TimeInterval.Months], Is.EqualTo(15));
            
            break;
          default:
            throw new ArgumentException("failed to parse interval");
            
            // break;
        }
        
      }
    }
    
    [Test]
    public void TestTryCityState()
    {
      string haystack = @"Request for Information - Sam's Club Niagara Falls, NY";
      
      System.Collections.Generic.List< System.Collections.Generic.Dictionary< string, string> > values;
      
      var result = parser.TryCityState(haystack, out values);
      
      Assert.That(result, Is.True);
      
      Assert.That( values.Count, Is.EqualTo(1) );
      
      var one = values[0];
      
      Assert.That( one["city"], Is.EqualTo("Niagara Falls") );
      
      Assert.That( one["state"], Is.EqualTo("NY") );
    }
    
    [Test]
    public void TestExtractRHAProgramNumber()
    {
      var projectNumber = 7704003;
      
      var result = parser.ExtractRhaProgramNumber(projectNumber);
      
      Assert.That( result, Is.EqualTo(4) );

      projectNumber = 7714563;
      
      result = parser.ExtractRhaProgramNumber(projectNumber);
      
      Assert.That( result, Is.EqualTo(14) );
      
      projectNumber = 0714063;
      
      result = parser.ExtractRhaProgramNumber(projectNumber);
      
      Assert.That( result, Is.EqualTo(14) );
      
      projectNumber = 802063;
      
      result = parser.ExtractRhaProgramNumber(projectNumber);
      
      Assert.That( result, Is.EqualTo(2) );
      
      projectNumber = 1303011;
      
      result = parser.ExtractRhaProgramNumber(projectNumber);
      
      Assert.That( result, Is.EqualTo(3) );
      
      projectNumber = 1414001;
      
      result = parser.ExtractRhaProgramNumber(projectNumber);
      
      Assert.That( result, Is.EqualTo(14) );
      
      projectNumber = 1514011;
      
      result = parser.ExtractRhaProgramNumber(projectNumber);
      
      Assert.That( result, Is.EqualTo(14) );
    }
    
    [Test]
    public void TestGetExtensionFromName ()
    {
      var name = "23.html.jpg";
      
      var expectedExtension = ".jpg";
      
      var result = parser.GetAttachmentExtension(name);
      
      Assert.That( result, Is.EqualTo(expectedExtension) );
      
      name = "45-dwg.ai";
      
      expectedExtension = ".ai";
      
      result = parser.GetAttachmentExtension(name);
      
      Assert.That( result, Is.EqualTo(expectedExtension) );
      
      name = "45.dwg.ai.1";
      
      expectedExtension = ".1";
      
      result = parser.GetAttachmentExtension(name);
      
      Assert.That( result, Is.EqualTo(expectedExtension) );
      
      name = "test-result*see>me.dwg.the=brown+dog.likes.to.jump(over)the$cat";
      
      expectedExtension = ".jump(over)the$cat";
      
      result = parser.GetAttachmentExtension(name);
      
      Assert.That( result, Is.EqualTo(expectedExtension) );
    }
    
    [Test]
    public void TestIsAnsweredTrue()
    {
      string needle = @"FW: RFI:Request for Information Answered: 5612-000 (Houston (Wayside), TX) RFI: 89";
      
      var result = parser.IsAnsweredQuestion(needle);
      
      Assert.That( result, Is.True);
    }
    
    [Test]
    public void TestIsAnsweredFalse()
    {
      string needle = @"FW: RFI:Request for Information Submitted: 7240-000 (ROWLETT, TX) RFI: 5";
      
      var result = parser.IsAnsweredQuestion(needle);
      
      Assert.That( result, Is.False);
    }
    
    [Test]
    public void TestIsValidRfiAttachmentNameTrue()
    {
      string needle = @"6188_RFI 050-Response Attachment 01.pdf";
      
      var result = parser.IsValidRfiAttachmentName(needle);
      
      Assert.That( result, Is.True);
      
      needle = @"6188_RFI 048_Response Attachment E4.1.pdf";
      
      result = parser.IsValidRfiAttachmentName(needle);
      
      Assert.That( result, Is.True);
      
      needle = @"6188_v05_Response Attachment 01.pdf";
      
      result = parser.IsValidRfiAttachmentName(needle);
      
      Assert.That( result, Is.True);
    }
    
        [Test]
    public void TestIsValidRfiAttachmentNameFalse()
    {
      string needle = @"draft RFI_Grease Interceptor.pdf";
      
      var result = parser.IsValidRfiAttachmentName(needle);
      
      Assert.That( result, Is.False);
      
      needle = @"6188 v09 - Response Attachment REM2.pdf";
      
      result = parser.IsValidRfiAttachmentName(needle);
      
      Assert.That( result, Is.False);
      
      needle = @"6188 RFI - ga2 grocery fixture plan.dwf";
      
      result = parser.IsValidRfiAttachmentName(needle);
      
      Assert.That( result, Is.False);
    }
  }
}
