using AutoFixture;
using BusinessLogic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Models;
using Moq;
using Service.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api_Testing
{
     public class PT_Test
    {
        private readonly IFixture fixture;
        private readonly Mock<ILogic> mlogic;
        private readonly PTestController pt;
        public PT_Test()
        {
            fixture = new Fixture();
            mlogic = fixture.Freeze<Mock<ILogic>>();
            pt = new PTestController(mlogic.Object);
        }

        [Fact]
        public void AddPT_OK_Test()
        {
            var hr = fixture.Create<Models.Patient_Test>();
            mlogic.Setup(x => x.AddTestReport(hr)).Returns(hr);

            var result = pt.Add(hr);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().NotBeNull().And.BeOfType(hr.GetType());
            mlogic.Verify(x => x.AddTestReport(hr), Times.AtLeastOnce());
        }
        [Fact]
        public void AddPT_Fail_Test()
        {
            var request = fixture.Create<Patient_Test>();
            mlogic.Setup(x => x.AddTestReport(request)).Throws(new Exception("Something wrong with the request"));


            var result = pt.Add(request);


            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestObjectResult>();
            mlogic.Verify(x => x.AddTestReport(request), Times.AtLeastOnce());
        }

        [Fact]
        public void UpdatePt_OK_Test()
        {
            var hr = fixture.Create<Models.Patient_Test>();
            var id = fixture.Create<string>();
            mlogic.Setup(x => x.UpdatePatientTest(id, hr)).Returns(hr);

            var result = pt.UpdateTestRecord(id, hr);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().NotBeNull().And.BeOfType(hr.GetType());
            mlogic.Verify(x => x.UpdatePatientTest(id, hr), Times.AtLeastOnce());
        }
    }
}
