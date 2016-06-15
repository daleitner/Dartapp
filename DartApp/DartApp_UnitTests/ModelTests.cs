using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using DartApp.Models;
using ExpectedObjects;
using NUnit.Framework;
using Should;
using SpecsFor;

namespace DartApp_UnitTests
{
	public class ModelTests
	{
		public class when_VorNameIsSet : SpecsFor<Player>
		{
			protected override void InitializeClassUnderTest()
			{
				this.SUT = new Player();
			}

			protected override void When()
			{
				this.SUT.VorName = "anyString";
			}

			[Test]
			public void then_DisplayNameShouldBeUpdated()
			{
				this.SUT.DisplayName.ShouldEqual("anyString");
			}
		}

		public class when_NachNameIsSet : SpecsFor<Player>
		{
			protected override void InitializeClassUnderTest()
			{
				this.SUT = new Player();
			}

			protected override void When()
			{
				this.SUT.NachName = "anyString";
			}

			[Test]
			public void then_DisplayNameShouldBeUpdated()
			{
				this.SUT.DisplayName.ShouldEqual("anyString");
			}
		}

		public class when_VorNameAndNachNameIsSet : SpecsFor<Player>
		{
			protected override void InitializeClassUnderTest()
			{
				this.SUT = new Player();
			}

			protected override void When()
			{
				this.SUT.VorName = "vname";
				this.SUT.NachName = "nname";
			}

			[Test]
			public void then_DisplayNameShouldBeUpdated()
			{
				this.SUT.DisplayName.ShouldEqual("nname vname");
			}
		}
	}
}
