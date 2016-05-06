using System;
using System.Text;
using System.Collections.Generic;
using DartApp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using SpecsFor;

namespace DartApp_UnitTests
{
	public class HomeViewModelTests
	{
		public class when_DatabaseButton_Clicked : SpecsFor<HomeViewModel>
		{
			protected override void When()
			{
				this.SUT.DataBaseCommand.Execute(null);
			}

			[Test]
			public void then_DisplayChangedEvent_should_be_thrown()
			{
				
			}
		}
	}
}
