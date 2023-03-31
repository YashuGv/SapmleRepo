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
    public class PA_Test
    {
        private readonly IFixture fixture;
        private readonly Mock<ILogic> mlogic;
        private readonly AllergyController pa;
        public PA_Test()
        {
            fixture = new Fixture();
            mlogic = fixture.Freeze<Mock<ILogic>>();
            pa = new AllergyController(mlogic.Object);
        }

        [Fact]
        public void AddPA_OK_Test()
        {
            var hr = fixture.Create<Models.Patient_Allergy>();
            mlogic.Setup(x => x.AddAllergyReport(hr)).Returns(hr);

            var result = pa.Add(hr);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().NotBeNull().And.BeOfType(hr.GetType());
            mlogic.Verify(x => x.AddAllergyReport(hr), Times.AtLeastOnce());
        }
        [Fact]
        public void AddPABadRequest_Test()
        {
            var request = fixture.Create<Patient_Allergy>();
            mlogic.Setup(x => x.AddAllergyReport(request)).Throws(new Exception("Something wrong with the request"));


            var result = pa.Add(request);


            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestObjectResult>();
            mlogic.Verify(x => x.AddAllergyReport(request), Times.AtLeastOnce());
        }

        [Fact]
        public void UpdateAllergyRecord_OK_Test()
        {
            var hr = fixture.Create<Models.Patient_Allergy>();
            var id = fixture.Create<string>();
            mlogic.Setup(x => x.UpdatePA(id, hr)).Returns(hr);

            var result = pa.UpdateAllergyRecord(id, hr);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().NotBeNull().And.BeOfType(hr.GetType());
            mlogic.Verify(x => x.UpdatePA(id, hr), Times.AtLeastOnce());
        }

        [Fact]
        public void UpdateAllergyRecord_BadRequest_Test()
        {
            var request = fixture.Create<Patient_Allergy>();
            var id = fixture.Create<string>();
            mlogic.Setup(x => x.UpdatePA(id, request)).Throws(new Exception("Something wrong with the request"));

            var result = pa.UpdateAllergyRecord(id, request);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestObjectResult>();
            mlogic.Verify(x => x.UpdatePA(id, request), Times.AtLeastOnce());

        }
    }
}
