using AutoFixture;
using BusinessLogic;
using Moq;
using Service.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using FluentAssertions;
using AutoFixture.Kernel;
using Models;
using EntityFrame;

namespace Api_Testing
{
    public class PH_Test
    {
        private readonly IFixture fixture;
        private readonly Mock<ILogic> mlogic;
        private readonly PHRecordController c1;
        public PH_Test()
        {
            fixture= new Fixture();
            mlogic= fixture.Freeze<Mock<ILogic>>();
            c1 = new PHRecordController(mlogic.Object);
        
        }
        [Fact]
        public void GetById_Test()
        {
            //Arrange
            var phrmock = fixture.Create<IEnumerable<EntityFrame.AllHealthDetails>>();
            var id = fixture.Create<string>();
            mlogic.Setup(x => x.GetByHealthID(id)).Returns(phrmock);
            //Act
            var result = c1.GetById(id);

            //Assert

            result.Should().NotBeNull();

            result.Should().BeAssignableTo<OkObjectResult>();

            result.As<OkObjectResult>().Value
                .Should().NotBeNull()
                .And.BeOfType(phrmock.GetType());

            mlogic.Verify(x => x.GetByHealthID(id), Times.AtLeastOnce());
        }

        [Fact]
        public void GetById_BadRequest_Test()
        {
            //Arrange
            IEnumerable<EntityFrame.AllHealthDetails> phrmock = null;
            var id = fixture.Create<string>();
            mlogic.Setup(x => x.GetByHealthID(id)).Returns(phrmock);
            //Act
            var result = c1.GetById(id);

            //Assert

            result.Should().BeAssignableTo<BadRequestObjectResult>();

            mlogic.Verify(x => x.GetByHealthID(id), Times.AtLeastOnce());
        }

        [Fact]
        public void AddHealthdetails_Test()
        {
            var hr = fixture.Create<Models.Patient_Health_Record>();
            mlogic.Setup(x => x.AddHealthR(hr)).Returns(hr);

            var result = c1.Add(hr);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().NotBeNull().And.BeOfType(hr.GetType());
            mlogic.Verify(x => x.AddHealthR(hr), Times.AtLeastOnce());
        }

        [Fact]
        public void AddHealthdetails_BadRequest_Test() {
            var request = fixture.Create<Patient_Health_Record>();
            mlogic.Setup(x => x.AddHealthR(request)).Throws(new Exception("Something wrong with the request"));


            var result = c1.Add(request);


            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestObjectResult>();
            mlogic.Verify(x => x.AddHealthR(request), Times.AtLeastOnce());
        }

        [Fact]
        public void UpdateHealthRecord_Test()
        {
            var hr = fixture.Create<Models.Patient_Health_Record>();
            var id = fixture.Create<string>();
            mlogic.Setup(x => x.UpdateHealthR(id,hr)).Returns(hr);

            var result = c1.UpdateHealthRecord(id,hr);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().NotBeNull().And.BeOfType(hr.GetType());
            mlogic.Verify(x => x.UpdateHealthR(id,hr), Times.AtLeastOnce());
        }

        [Fact]
        public void UpdateHealthRecord_BadRequest_Test() {
            var request = fixture.Create<Patient_Health_Record>();
            var id = fixture.Create<string>();
            mlogic.Setup(x => x.UpdateHealthR(id, request)).Throws(new Exception("Something wrong with the request"));

            var result = c1.UpdateHealthRecord(id, request);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestObjectResult>();
            mlogic.Verify(x => x.UpdateHealthR(id, request), Times.AtLeastOnce());
        }
    }
}