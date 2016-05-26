using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DartApp;
using DartApp.Home;
using DartApp.CommandServices;
using Moq;
using NUnit.Framework;
using Should;
using SpecsFor;


namespace DartApp_UnitTests
{
	public class MainViewModelTests
	{
		public class when_Start_Application : SpecsFor<MainViewModel>
		{
			[Test]
			public void then_HomeViewModel_IsDisplayed()
			{
				this.SUT.Content.ShouldBeType(typeof (HomeViewModel));
			}

			[Test]
			public void then_InitializeDataBase_IsCalled()
			{
				GetMockFor<IDartAppCommandService>().Verify(x => x.InitializeDatabase(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
			}
		}
	}
}
