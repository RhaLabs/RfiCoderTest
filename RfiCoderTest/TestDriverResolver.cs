/*
 * Created by SharpDevelop.
 * User: bcrawford
 * Date: 9/15/2014
 * Time: 4:53 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using NUnit.Framework;
using RfiCoder.Utilities;

namespace RfiCoderTest
{
  [TestFixture]
  public class TestDriverResolver
  {
    [Test]
    public void TestIsNetworkDriveTrue()
    {
      var path = @"R:\";
      
      var result = DriveResolver.isNetworkDrive(path);
      
      Assert.That(result, Is.True);
      
      path = @"\\W2008SVR6\";
      
      result = DriveResolver.isNetworkDrive(path);
      
      Assert.That(result, Is.True);
    }
    
    [Test]
    public void TestIsNetworkDriveFalse()
    {
      var path = @"C:\";
      
      var result = DriveResolver.isNetworkDrive(path);
      
      Assert.That(result, Is.False);
    }
    
    [Test]
    public void TestGetDriveLetterError()
    {
      var path = @"\\W2008SVR6";
      
      Assert.Throws< ArgumentException >( () => DriveResolver.GetDriveLetter(path) );
    }
    
    [Test]
    public void TestGetDriveLetterTrue()
    {
      var path = @"C:\";
      
      Assert.That( DriveResolver.GetDriveLetter(path), Is.EqualTo("C:") );
      
      path = @"R:\";
      
      Assert.That(DriveResolver.GetDriveLetter(path), Is.EqualTo("R:") );
    }
    
    [Test]
    public void TestResolveToRootUNC()
    {
      var path = @"C:\Windows";
      
      Assert.That( DriveResolver.ResolveToRootUNC(path), Is.EqualTo(@"C:\") );
      
      path = @"R:\04 new Sams";
      
      Assert.That(DriveResolver.ResolveToRootUNC(path), Is.EqualTo(@"\\w2012svr1\R") );
      
      path = @"\\W2008SVR7\R\04 new Sams";
      
      Assert.That(DriveResolver.ResolveToRootUNC(path), Is.EqualTo(@"\\w2012svr1\R") );
    }
    
    [Test]
    public void TestResolveToUNC()
    {
      var path = @"C:\Windows";
      
      Assert.That( DriveResolver.ResolveToUNC(path), Is.EqualTo(@"C:\Windows") );
      
      path = @"R:\04 new Sams";
      
      Assert.That(DriveResolver.ResolveToUNC(path), Is.EqualTo(@"\\w2012svr1\R\04 new Sams") );
      
      path = @"\\W2008SVR7\R\04 new Sams";
      
      Assert.That(DriveResolver.ResolveToUNC(path), Is.EqualTo(@"\\w2012svr1\R\04 new Sams") );
    }

  }
}
