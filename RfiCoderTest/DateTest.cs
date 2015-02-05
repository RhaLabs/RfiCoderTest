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
  public class DateTest
  {
    [Test]
    public void TestWeekends()
    {
      var days = new System.Collections.Generic.List< System.DateTime >{
        DateTime.Parse("Sun, 14 Sep 2008 00:00:00 GMT"),
        DateTime.Parse("Mon, 15 Sep 2008 00:00:00 GMT"),
        DateTime.Parse("Tue, 16 Sep 2008 00:00:00 GMT"),
        DateTime.Parse("Wed, 17 Sep 2008 00:00:00 GMT"),
        DateTime.Parse("Thu, 18 Sep 2008 00:00:00 GMT"),
        DateTime.Parse("Fri, 19 Sep 2008 00:00:00 GMT"),
        DateTime.Parse("Sat, 20 Sep 2008 00:00:00 GMT")
      };
      
      foreach (var day in days) {
        switch (day.DayOfWeek) {
          case DayOfWeek.Sunday:
            Assert.That(
              RfiCoder.Utilities.DateTime.AdjustForWeekend(day).DayOfWeek,
              Is.EqualTo(System.DayOfWeek.Monday)
             );
            break;
            
          case DayOfWeek.Saturday:
            Assert.That(
              RfiCoder.Utilities.DateTime.AdjustForWeekend(day).DayOfWeek,
              Is.EqualTo(System.DayOfWeek.Friday)
             );
            break;
            
          default:
            Assert.That(
              RfiCoder.Utilities.DateTime.AdjustForWeekend(day).DayOfWeek,
              Is.EqualTo(day.DayOfWeek)
             );
            break;
        }
      }
      
    }
    
    [Test]
    public void TestHolidays()
    {
      var holidays = RfiCoder.Utilities.DateTime.GetHolidays(2000);
      
      var days = new System.Collections.Generic.List< System.DateTime >{
        DateTime.Parse("1999-12-31 00:00:00"), // New Year's Day
        DateTime.Parse("2000-1-17 00:00:00"), // Martin Luther King Day
        DateTime.Parse("2000-2-21 00:00:00"), //President's Day
        DateTime.Parse("2000-5-29 00:00:00"), // Mwmorial Day
        DateTime.Parse("2000-7-4 00:00:00"), // Independence Day
        DateTime.Parse("2000-9-4 00:00:00"), //Labor Day
        DateTime.Parse("2000-10-9 00:00:00"), // Columbus Day
        DateTime.Parse("2000-11-10 00:00:00"), //Veteran's Day
        DateTime.Parse("2000-11-23 00:00:00"), // Thanksgiving Day,
        DateTime.Parse("2000-12-25 00:00:00") // Christmas Day
      };
      
      foreach (var holiday in holidays) {
        Assert.That(days.Contains(holiday), Is.True);
      }
    }
    
    [Test]
    public void TestDueDateHours()
    {
      var startDate = System.DateTime.Parse("Mon, 10 May 2010");
      
      var interval = "26 Hours";
      
      var dueDate = System.DateTime.Parse("Mon, 10 May 2010");
      
      Assert.That(
        RfiCoder.Utilities.DateTime.DueDate(
          startDate, interval),
        Is.EqualTo(dueDate.AddHours(26d))
       );
    }
    
    [Test]
    public void TestDueDateWeekday()
    {
      var startDate = System.DateTime.Parse("Mon, 10 May 2010");
      
      var interval = "3 Days";
      
      var dueDate = System.DateTime.Parse("Thu, 13 May 2010");
      
      Assert.That(
        RfiCoder.Utilities.DateTime.DueDate(
          startDate, interval),
        Is.EqualTo(dueDate)
       );
    }
    
    [Test]
    public void TestDueDateSaturday()
    {
      var startDate = System.DateTime.Parse("Wed, 12 May 2010");
      
      var interval = "3 Days";
      
      var dueDate = System.DateTime.Parse("Fri, 14 May 2010");
      
      Assert.That(
        RfiCoder.Utilities.DateTime.DueDate(
          startDate, interval),
        Is.EqualTo(dueDate)
       );
    }
    
    [Test]
    public void TestDueDateSunday()
    {
      var startDate = System.DateTime.Parse("Thu, 13 May 2010");
      
      var interval = "3 Days";
      
      var dueDate = System.DateTime.Parse("Mon, 17 May 2010");
      
      Assert.That(
        RfiCoder.Utilities.DateTime.DueDate(
          startDate, interval),
        Is.EqualTo(dueDate)
       );
    }
    
    [Test]
    public void TestIsOverdue()
    {
      var startDate = System.DateTime.Now.AddHours(-86);
      
      var dueDate = startDate.AddHours(72);
      
      System.TimeSpan elaspedTime;
      
      Assert.That(
        RfiCoder.Utilities.DateTime.IsOverdue(
          startDate, dueDate, out elaspedTime),
        Is.True
       );
      
      Assert.That(elaspedTime.TotalHours, Is.InRange(13.999, 14.00001));
    }
    
    [Test]
    public void TestIsNotOverdue()
    {
      var startDate = System.DateTime.Now;
      
      var dueDate = startDate.AddHours(72);
      
      System.TimeSpan elaspedTime;
      
      Assert.That(
        RfiCoder.Utilities.DateTime.IsOverdue(
          startDate, dueDate, out elaspedTime),
        Is.False
       );
      
      var expected = -72d;
      
      var tolerance = 0.1111111d;
      
      Assert.That(elaspedTime.TotalHours,
                  Is.InRange(expected - tolerance, expected + tolerance)
                 );
    }

  }
  
}
