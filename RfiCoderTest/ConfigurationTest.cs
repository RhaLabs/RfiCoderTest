/*
 * Created by SharpDevelop.
 * User: bcrawford
 * Date: 8/8/2014
 * Time: 8:30 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Linq;
using RfiCoder.Utilities;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace RfiCoderTest
{
[TestFixture]
  class ConfigurationTest
  {
    
    [Test]
    public void TestConfigHandler ()
    {
      var handler = RfiCoder.Configuration.ConfigHandler.InstanceOf;
      
      Assert.That(handler, Is.InstanceOf<RfiCoder.Configuration.ConfigHandler>());
      
      handler.SaveConfig();
    }
    
    [Test]
    public void TestReadJson ()
    {
      var handler = RfiCoder.Configuration.ConfigHandler.InstanceOf;
      
      var programs = handler.ProgramMappings;
      
      var fuelStation = programs[14];
      
      Assert.That(fuelStation, Is.EqualTo( RfiCoder.Enum.ProgramTypes.FuelStations ));
    }
    
    [Test]
    public void WriteTest ()
    {
      var handler = RfiCoder.Configuration.ConfigHandler.InstanceOf;
      
      var list = new System.Collections.Generic.List< System.Collections.Generic.KeyValuePair< RfiCoder.Enum.ProgramTypes, string > > ();
      
      list.Add( new System.Collections.Generic.KeyValuePair< RfiCoder.Enum.ProgramTypes, string > (RfiCoder.Enum.ProgramTypes.NewStores, @"R:\04 New Sams") );
      list.Add( new System.Collections.Generic.KeyValuePair< RfiCoder.Enum.ProgramTypes, string > (RfiCoder.Enum.ProgramTypes.ABRHoldings, @"V:\16 ABR Holdings") );
      list.Add(new System.Collections.Generic.KeyValuePair< RfiCoder.Enum.ProgramTypes, string > (RfiCoder.Enum.ProgramTypes.Express, @"R:\17 XPS") );
      list.Add(new System.Collections.Generic.KeyValuePair< RfiCoder.Enum.ProgramTypes, string > (RfiCoder.Enum.ProgramTypes.NewStores, @"R:\05 New Supercenters") );
      
      handler.FileServerMappings = list;
      
      handler.SaveConfig(@".\\test.json");
    }

  }
}