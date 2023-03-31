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
    public class PM_Test
    {
        private readonly IFixture fixture;
        private readonly Mock<ILogic> mlogic;
        private readonly MedicationController pm;
        public PM_Test()
        {
            fixture = new Fixture();
            mlogic = fixture.Freeze<Mock<ILogic>>();
            pm= new MedicationController(mlogic.Object);
        }

        [Fact]
        public void AddPM_Test()
        {
            var hr = fixture.Create<Models.Patient_Medication>();
            mlogic.Setup(x => x.AddMedicalReport(hr)).Returns(hr);

            var result = pm.Add(hr);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().NotBeNull().And.BeOfType(hr.GetType());
            mlogic.Verify(x => x.AddMedicalReport(hr), Times.AtLeastOnce());
        }
        [Fact]
        public void AddPM_BadRequest_Test()
        {
            var request = fixture.Create<Patient_Medication>();
            mlogic.Setup(x => x.AddMedicalReport(request)).Throws(new Exception("Something wrong with the request"));


            var result = pm.Add(request);


            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestObjectResult>();
            mlogic.Verify(x => x.AddMedicalReport(request), Times.AtLeastOnce());
        }

        [Fact]
        public void UpdateMedicalRecord_OK_Test()
        {
            var hr = fixture.Create<Models.Patient_Medication>();
            var id = fixture.Create<string>();
            mlogic.Setup(x => x.UpdateMedicalReport(id, hr)).Returns(hr);

            var result = pm.UpdateMedicalRecord(id, hr);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().NotBeNull().And.BeOfType(hr.GetType());
            mlogic.Verify(x => x.UpdateMedicalReport(id, hr), Times.AtLeastOnce());
        }

        [Fact]
        public void UpdateMedicalRecord_BadRequest_Test()
        {
            var request = fixture.Create<Patient_Medication>();
            var id = fixture.Create<string>();
            mlogic.Setup(x => x.UpdateMedicalReport(id, request)).Throws(new Exception("Something wrong with the request"));

            var result = pm.UpdateMedicalRecord(id, request);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestObjectResult>();
            mlogic.Verify(x => x.UpdateMedicalReport(id, request), Times.AtLeastOnce());
            
        }
    }
}
