/*
 * Created by SharpDevelop.
 * User: bcrawford
 * Date: 5/16/2014
 * Time: 7:55 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace RfiCoderTest
{
  /// <summary>
  /// Description of DataconnectorTest.
  /// </summary>
  [TestFixture]
  public class DatabaseConnectorTest
  {
    
    [Test]
    public void TestGetStore()
    {
      var databaseConnector = new RfiCoder.Data.DatabaseConnector();
      
      var store = databaseConnector.GetStore(1536);
      
      Assert.That(store.Count, Is.AtLeast(1));
      
      Assert.That(store[0].Number, Is.EqualTo(1536));
    }
    
    [Test]
    public void TestGetStoreWithSequence()
    {
      var databaseConnector = new RfiCoder.Data.DatabaseConnector();
      
      var aStore = databaseConnector.GetStore(1536, 5);
      
      Assert.That(aStore.Count, Is.EqualTo(1));
      
      Assert.That(aStore[0].Number, Is.EqualTo(1536));
      
      Assert.That(aStore[0].Type, Is.EqualTo(RfiCoder.Enum.ProjectTypes.Relo));
      
      Assert.That( aStore[0].Program, Is.EqualTo(RfiCoder.Enum.ProgramTypes.NewStores) );
    }
    
    [Test]
    public void TestGetStoreWithSequence2()
    {
      var databaseConnector = new RfiCoder.Data.DatabaseConnector();
      
      var aStore = databaseConnector.GetStore(2363, 0);
      
      Assert.That(aStore.Count, Is.EqualTo(1));
      
      Assert.That(aStore[0].Number, Is.EqualTo(2363));
      
      Assert.That(aStore[0].Type, Is.EqualTo(RfiCoder.Enum.ProjectTypes.GroundUp));
      
      Assert.That( aStore[0].Program, Is.EqualTo(RfiCoder.Enum.ProgramTypes.Express) );
    }
    
    [Test]
    public void TestGetProjectContacts()
    {
      var databaseConnector = new RfiCoder.Data.DatabaseConnector();
      
      var aStore = databaseConnector.GetStore(1536, 5);
      
      var result = databaseConnector.GetProjectContacts(aStore[0]);
      
      Assert.That(result.Count, Is.GreaterThan(0));
    }
    
    [Test]
    public void TestDiscoverRfi()
    {
      var email = new Microsoft.Exchange.WebServices.Data.EmailMessage(
        RfiCoder.Utilities.EmailHelper.InstanceOf.GetEmailService);
      
      email.Subject = @"WM Store Sink Drain Dimensions RFI";
      
      email.Body = @"> From: Jim Rich
> Sent: Tuesday, June 10, 2014 12:31 PM
> To: pesebio@rhaaia.com<mailto:pesebio@rhaaia.com>
> Cc: leonardgabaldon@mickrichcontractors.com<mailto:leonardgabaldon@mickrichcontractors.com>
> Subject: WM Store Sink Drain Dimensions RFI
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
      
      Assert.That(store.City, Is.Not.Null);
    }
    
    [Test]
    public void TestGetAllStoresByCityState()
    {
      string city = "Dallas";
      
      string state = "TX";
      
      var databaseConnector = new RfiCoder.Data.DatabaseConnector();
      
      var store = databaseConnector.GetAllStoresByCityState(city, state);
      
      Assert.That(store.Count, Is.AtLeast(1));
      
      Assert.That(
        store.Exists( x => x.City == "Dallas" )
        , Is.True );
    }
  }
}
