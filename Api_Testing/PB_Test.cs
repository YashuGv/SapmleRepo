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
    public class PB_Test
    {
        private readonly IFixture fixture;
        private readonly Mock<ILogic> mlogic;
        private readonly PBRecordController c1;
        public PB_Test()
        {
            fixture = new Fixture();
            mlogic = fixture.Freeze<Mock<ILogic>>();
            c1 = new PBRecordController(mlogic.Object);

        }
        [Fact]
        public void GetById_Test()
        {
            //Arrange
            var phrmock = fixture.Create<IEnumerable<EntityFrame.AllBasicDetails>>();
            var id = fixture.Create<string>();
            mlogic.Setup(x => x.GetById(id)).Returns(phrmock);
            //Act
            var result = c1.GetById(id);

            //Assert

            result.Should().NotBeNull();

            result.Should().BeAssignableTo<OkObjectResult>();

            result.As<OkObjectResult>().Value
                .Should().NotBeNull()
                .And.BeOfType(phrmock.GetType());

            mlogic.Verify(x => x.GetById(id), Times.AtLeastOnce());
        }

        [Fact]
        public void GetById_BadRequest_Test()
        {
            //Arrange
            IEnumerable<EntityFrame.AllBasicDetails> phrmock = null;
            var id = fixture.Create<string>();
            mlogic.Setup(x => x.GetById(id)).Returns(phrmock);
            //Act
            var result = c1.GetById(id);

            //Assert

            result.Should().BeAssignableTo<BadRequestObjectResult>();

            mlogic.Verify(x => x.GetById(id), Times.AtLeastOnce());
        }

        [Fact]
        public void AddBasicdetails_Test()
        {
            var hr = fixture.Create<Models.Patient_Basic_Record>();
            mlogic.Setup(x => x.AddBasicR(hr)).Returns(hr);

            var result = c1.Add(hr);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().NotBeNull().And.BeOfType(hr.GetType());
            mlogic.Verify(x => x.AddBasicR(hr), Times.AtLeastOnce());
        }

        [Fact]
        public void AddBasicdetails_BadRequest_Test()
        {
            var request = fixture.Create<Patient_Basic_Record>();
            mlogic.Setup(x => x.AddBasicR(request)).Throws(new Exception("Something wrong with the request"));


            var result = c1.Add(request);


            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestObjectResult>();
            mlogic.Verify(x => x.AddBasicR(request), Times.AtLeastOnce());
        }

        [Fact]
        public void UpdateBasicrecord_Test()
        {
            var hr = fixture.Create<Models.Patient_Basic_Record>();
            var id = fixture.Create<string>();
            mlogic.Setup(x => x.UpdateBR(id, hr)).Returns(hr);

            var result = c1.UpdateBasicRecord(id, hr);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().NotBeNull().And.BeOfType(hr.GetType());
            mlogic.Verify(x => x.UpdateBR(id, hr), Times.AtLeastOnce());
        }

        [Fact]
        public void UpdateBasicRecord_BadRequest_Test()
        {
            var request = fixture.Create<Patient_Basic_Record>();
            var id = fixture.Create<string>();
            mlogic.Setup(x => x.UpdateBR(id, request)).Throws(new Exception("Something wrong with the request"));

            var result = c1.UpdateBasicRecord(id, request);

            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestObjectResult>();
            mlogic.Verify(x => x.UpdateBR(id, request), Times.AtLeastOnce());
        }
    }
}
