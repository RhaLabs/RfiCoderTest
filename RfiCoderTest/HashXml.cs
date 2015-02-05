/*
 * Created by SharpDevelop.
 * User: bcrawford
 * Date: 9/3/2014
 * Time: 9:15 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace RfiCoderTest
{
  /// <summary>
  /// Description of HashXml.
  /// </summary>
  public class HashXml : RfiCoder.Data.HashXmlStorage
  {
    public HashXml(string path)
      : base (path)
    {
    }
    
    public string Path
    {
      get { return base.Path; }
    }
    
    public string FileName
    {
      get { return base.FileName; }
    }
  }
}
