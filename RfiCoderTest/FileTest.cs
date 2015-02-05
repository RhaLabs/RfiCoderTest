/*
 * Created by SharpDevelop.
 * User: bcrawford
 * Date: 9/2/2014
 * Time: 12:46 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using NUnit.Framework;

namespace RfiCoderTest
{
  [TestFixture]
  public class FileTest
  {
    [Test]
    public void TestGetPathToProject()
    {
      var store = new RfiCoder.Entity.Store();
      
      store.Program = RfiCoder.Enum.ProgramTypes.NewStores;
      
      store.ProjectNumber = 1404016;
      
      store.Type = RfiCoder.Enum.ProjectTypes.GroundUp;
      
      store.Number = 85527;
      
      store.City = "Broomfield";
      
      var filehandler = new RfiCoder.Utilities.FileHandler();
      
      var result = filehandler.GetFileServerProjectPath( store );
      
      Assert.That( result, Is.Not.Empty );
      
      Assert.That( result, Is.EqualTo(@"R:\04 New Sams\Broomfield, CO #85527 (1404016)\14-RFI Attachments") );
    }
    
    [Test]
    public void TestGetPathToAProject()
    {
      var store = new RfiCoder.Entity.Store();
      
      store.Program = RfiCoder.Enum.ProgramTypes.NewStores;
      
      store.ProjectNumber = 1105039;
      
      store.Type = RfiCoder.Enum.ProjectTypes.GroundUp;
      
      store.Number = 4165;
      
      store.City = "Fort Worth (Jackson & River Oaks), TX #4165 (1105039)";
      
      var filehandler = new RfiCoder.Utilities.FileHandler();
      
      var result = filehandler.GetFileServerProjectPath( store );
      
      Assert.That( result, Is.Not.Empty );
      
      Assert.That( result, Is.EqualTo(@"R:\05 New Supercenters\Fort Worth (Jackson & River Oaks), TX #4165 (1105039)\14-RFI Attachments") );
    }
    
    [Test]
    public void TestTruthFileComparison ()
    {
      var directory = new System.IO.DirectoryInfo(System.Environment.SystemDirectory);
      
      var files = directory.GetFiles();
      
      var randGen = new System.Random();
      
      var pt = randGen.Next(1, files.Length);
      
      var file = files[pt];
      
      var fileHandler = new RfiCoder.Utilities.FileHandler();
      
      var result = fileHandler.BitwiseFileComaparison(file, file);
      
      Assert.That( result, Is.True );
    }
    
    [Test]
    public void TestFalseFileComparison ()
    {
      var directory = new System.IO.DirectoryInfo(System.Environment.SystemDirectory);
      
      var files = directory.GetFiles();
      
      var randGen = new System.Random();
      
      var pt1 = randGen.Next(1, files.Length);
      
      var file1 = files[pt1];
      
      var pt2 = randGen.Next(pt1, files.Length);
      
      var file2 = files[pt2];
      
      var fileHandler = new RfiCoder.Utilities.FileHandler();
      
      var result = fileHandler.BitwiseFileComaparison(file1, file2);
      
      Assert.That( result, Is.False );
    }
    
    [Test]
    public void TestHashXmlStoragePathing ()
    {
      var hasher = new HashXml(@"..\XmlStorage\test.xml");
      
      Assert.That(hasher.FileName, Is.EqualTo("test.xml"));
      
      Assert.That(hasher.Path, Is.EqualTo(@"..\XmlStorage"));
    }
    
    [Test]
    public void TestDirectoryHashing()
    {
      var directory = new System.IO.DirectoryInfo(System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));
      
      var fileHandler = new RfiCoder.Utilities.FileHandler();
      
      var result = fileHandler.DirectoryFileHash(directory);
      
      Assert.That(result.Result.Count, Is.GreaterThan(0));
    }
    
    [Test]
    public void TestHashStorage()
    {
      var directory = new System.IO.DirectoryInfo(System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));
      
      var fileHandler = new RfiCoder.Utilities.FileHandler();
      
      var hasher = new RfiCoder.Data.HashStorage(@"..\XmlStorage\test.xml");
      
      hasher.Read();
      
      var result = fileHandler.DirectoryFileHash(directory);
      
      var store = new RfiCoder.Entity.Store();
      
      store.ProjectNumber = 1420563;
      
      store.City = "South Boston";
      
      hasher.Write(directory, result.Result, store);
    }
    
    [Test]
    public void TestSaveAttachmentsToEmptyFolder()
    {
      var directory = new System.IO.DirectoryInfo(System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));

      var store = new RfiCoder.Entity.Store();
      
      store.ProjectNumber = 1305015;
      
      store.Program = RfiCoder.Enum.ProgramTypes.NewStores;
      
      store.City = "Oconomowoc";
      
      var fileHandler = new RfiCoder.Utilities.FileHandler(store);
      
      var email = new Microsoft.Exchange.WebServices.Data.EmailMessage(
        RfiCoder.Utilities.EmailHelper.InstanceOf.GetEmailService);
      
      email.Subject = @"WM Store #9999 Sink Drain Dimensions RFI";
      
      var files = directory.GetFiles();
      
      var randGen = new System.Random();
      
      var pt = randGen.Next(1, files.Length);
      
      var file = files[pt];
      
      email.Attachments.AddFileAttachment(file.Name, file.OpenRead());
      
      email.Save(Microsoft.Exchange.WebServices.Data.WellKnownFolderName.Drafts);
      
      var result = fileHandler.SaveAttachmentsToFolder(email.Attachments);
      
      email.Delete(Microsoft.Exchange.WebServices.Data.DeleteMode.HardDelete);
    }
    
    [Test]
    public void TestSaveAttachmentsToFolder()
    {
      var directory = new System.IO.DirectoryInfo(System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));

      var store = new RfiCoder.Entity.Store();
      
      store.ProjectNumber = 1304007;
      
      store.Program = RfiCoder.Enum.ProgramTypes.NewStores;
      
      store.City = "Austin (lakeline)";
      
      var fileHandler = new RfiCoder.Utilities.FileHandler(store);
      
      var email = new Microsoft.Exchange.WebServices.Data.EmailMessage(
        RfiCoder.Utilities.EmailHelper.InstanceOf.GetEmailService);
      
      email.Subject = @"WM Store #9999 Sink Drain Dimensions RFI";
      
      var files = directory.GetFiles();
      
      var randGen = new System.Random();
      
      var pt = randGen.Next(1, files.Length);
      
      var file = files[pt];
      
      email.Attachments.AddFileAttachment(file.Name, file.OpenRead());
      
      email.Save(Microsoft.Exchange.WebServices.Data.WellKnownFolderName.Drafts);
      
      var result = fileHandler.SaveAttachmentsToFolder(email.Attachments);
      
      email.Delete(Microsoft.Exchange.WebServices.Data.DeleteMode.HardDelete);
      
      System.IO.File.Delete( String.Format("{0}\\{1}", fileHandler.Path, file.Name) );
      
      Assert.That(result, Is.True);
    }
    
    [Test]
    public void TestSaveAttachmentsToFolderWithDuplicateFile()
    {
      var store = new RfiCoder.Entity.Store();
      
      store.ProjectNumber = 1304007;
      
      store.Program = RfiCoder.Enum.ProgramTypes.NewStores;
      
      store.City = "Austin (lakeline)";
      
      var fileHandler = new RfiCoder.Utilities.FileHandler(store);
      
      var email = new Microsoft.Exchange.WebServices.Data.EmailMessage(
        RfiCoder.Utilities.EmailHelper.InstanceOf.GetEmailService);
      
      email.Subject = @"WM Store #9999 Sink Drain Dimensions RFI";
      
      var directory = new System.IO.DirectoryInfo(fileHandler.Path);
      
      var files = directory.GetFiles();
      
      var randGen = new System.Random();
      
      var pt = randGen.Next(1, files.Length);
      
      var file = files[pt];
      
      email.Attachments.AddFileAttachment(file.Name, file.OpenRead());
      
      email.Save(Microsoft.Exchange.WebServices.Data.WellKnownFolderName.Drafts);
      
      var result = fileHandler.SaveAttachmentsToFolder(email.Attachments);
      
      email.Delete(Microsoft.Exchange.WebServices.Data.DeleteMode.HardDelete);
      
      Assert.That(result, Is.False);
    }
    
    [Test]
    public void TestSaveAttachmentsAndEmail()
    {
      int i = 0;
      
      var directory = new System.IO.DirectoryInfo(System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory));

      var store = new RfiCoder.Entity.Store();
      
      store.ProjectNumber = 1305015;
      
      store.Program = RfiCoder.Enum.ProgramTypes.NewStores;
      
      store.City = "Oconomowoc";
      
      do {
        var email = new Microsoft.Exchange.WebServices.Data.EmailMessage(
          RfiCoder.Utilities.EmailHelper.InstanceOf.GetEmailService);
        
        email.Subject = @"FW: RFI:Request for Information Answered: TEST";
        
        email.Categories.Add("TEST");
        
        var files = directory.GetFiles();
        
        var randGen = new System.Random();
        
        var pt = randGen.Next(1, files.Length);
        
        var file = files[pt];
        
        email.Attachments.AddFileAttachment(file.Name, file.OpenRead());
        
        email.Save(RfiCoder.Utilities.EmailHelper.InstanceOf.NewStoresFolder);
        
        i ++;
        
      } while ( i < 5 );
      
      var itemView = new Microsoft.Exchange.WebServices.Data.ItemView(int.MaxValue);
      
      itemView.PropertySet = new Microsoft.Exchange.WebServices.Data.PropertySet(Microsoft.Exchange.WebServices.Data.BasePropertySet.IdOnly, Microsoft.Exchange.WebServices.Data.ItemSchema.Categories);
      
      var filter = new Microsoft.Exchange.WebServices.Data.SearchFilter.IsEqualTo(Microsoft.Exchange.WebServices.Data.EmailMessageSchema.Categories, "TEST");
      
      var results = RfiCoder.Utilities.EmailHelper.InstanceOf.GetEmailService.FindItems(RfiCoder.Utilities.EmailHelper.InstanceOf.NewStoresFolder, filter, itemView);
      
      var message = Microsoft.Exchange.WebServices.Data.Item.Bind(RfiCoder.Utilities.EmailHelper.InstanceOf.GetEmailService,
                                        results.Items[0].Id,
                                        RfiCoder.Utilities.EmailHelper.EmailProperties
                                       );
      
      var result = RfiCoder.Utilities.EmailHelper.InstanceOf.ArchiveAnsweredRfi(message, store);
      
      Assert.That(result, Is.True);
    }
  }
}
