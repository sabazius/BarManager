﻿using BarManager.Models.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using AutoMapper;
using BarManager.BL.Interfaces;
using BarManager.BL.Services;
using BarManager.Controllers;
using BarManager.DL.Interfaces;
using BarManager.Extensions;
using BarManager.Models.Requests;
using BarManager.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Serilog;
using Xunit;
using BarManager.Host.Controllers;

namespace BarManager.Test
{
    public class EmployeeTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IEmployeeRepository> _employeeRepository;
        private readonly IEmployeeService _employeeService;
        private readonly EmployeeController _employeeController;

        private IList<Employee> Employees = new List<Employee>()
        {
            new Employee()
            {
                Id = 1,
                Name = "Ivan",
                ClientTable = new List<int>(),
                Type = Models.Enums.EmployeeType.Waiter
            },
            new()
        {
            Id = 2,
            Name = "Dragan",
            ClientTable = new List<int>(),
            Type = Models.Enums.EmployeeType.Manager
        } };

        public EmployeeTests()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapping());
            });
            _mapper = mockMapper.CreateMapper();

            _employeeRepository = new Mock<IEmployeeRepository>();

            var logger = new Mock<ILogger>();

            _employeeService = new EmployeeService(_employeeRepository.Object);
            _employeeController = new EmployeeController(_employeeService, _mapper);

        }
        [Fact]
        public void Employee_GetAll_Count_Check()
        {
            //setup
            var expectedCount = 2;

            var mockedService = new Mock<IEmployeeService>();

            mockedService.Setup(x => x.GetAll()).Returns(Employees);

            //inject
            var controller = new EmployeeController(mockedService.Object, _mapper);

            //Act
            var result = controller.GetAll();

            //Assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.Equal(okObjectResult.StatusCode, (int)HttpStatusCode.OK);

            var employees= okObjectResult.Value as IEnumerable<Employee>;
            Assert.NotNull(employees);
            Assert.Equal(expectedCount, employees.Count());
        }

        [Fact]
        public void Employee_GetById_NameCheck()
        {
            //setup
            var employeeId = 2;
            var expectedName = "Dragan";

            _employeeRepository.Setup(x => x.GetById(employeeId))
                .Returns(Employees.FirstOrDefault(e => e.Id == employeeId));

            //Act
            var result = _employeeController.GetById(employeeId);

            //Assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.Equal(okObjectResult.StatusCode, (int)HttpStatusCode.OK);

            var response = okObjectResult.Value as EmployeeResponse;
            var employee = _mapper.Map<Employee>(response);

            Assert.NotNull(employee);
            Assert.Equal(expectedName, employee.Name);
        }

        [Fact]
        public void Employee_GetById_NotFound()
        {
            //setup
            var employeeId = 3;

            _employeeRepository.Setup(x => x.GetById(employeeId))
                .Returns(Employees.FirstOrDefault(t => t.Id == employeeId));

            //Act
            var result = _employeeController.GetById(employeeId);

            //Assert
            var notFoundObjectResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundObjectResult);
            Assert.Equal(notFoundObjectResult.StatusCode, (int)HttpStatusCode.NotFound);

            var response = (int)notFoundObjectResult.Value;
            Assert.Equal(employeeId, response);
        }

        [Fact]
        public void Employee_Update_EmployeeName()
        {
            var employeeId = 1;
            var expectedName = "Updated Employee Name";

            var employee = Employees.FirstOrDefault(x => x.Id == employeeId);
            employee.Name = expectedName;

            _employeeRepository.Setup(x => x.GetById(employeeId))
                .Returns(Employees.FirstOrDefault(t => t.Id == employeeId));
            _employeeRepository.Setup(x => x.Update(employee))
                .Returns(employee);

            //Act
            var employeeUpdateRequest = _mapper.Map<EmployeeUpdateRequest>(employee);
            var result = _employeeController.Update(employeeUpdateRequest);

            //Assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.Equal(okObjectResult.StatusCode, (int)HttpStatusCode.OK);

            var val = okObjectResult.Value as Employee;
            Assert.NotNull(val);
            Assert.Equal(expectedName, val.Name);

        }

        [Fact]
        public void Employee_Delete_ExistingEmployee()
        {
            //Setup
            var employeeId = 1;

            var employee = Employees.FirstOrDefault(x => x.Id == employeeId);

            _employeeRepository.Setup(x => x.Delete(employeeId)).Callback(() => Employees.Remove(employee)).Returns(employee);

            //Act
            var result = _employeeController.Delete(employeeId);

            //Assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.Equal(okObjectResult.StatusCode, (int)HttpStatusCode.OK);

            var val = okObjectResult.Value as Employee;
            Assert.NotNull(val);
            Assert.Equal(1, Employees.Count);
            Assert.Null(Employees.FirstOrDefault(x => x.Id == employeeId));
        }

        [Fact]
        public void Employee_Delete_NotExisting_Employee()
        {
            //Setup
            var employeeId = 3;

            var employee = Employees.FirstOrDefault(x => x.Id == employeeId);

            _employeeRepository.Setup(x => x.Delete(employeeId)).Callback(() => Employees.Remove(employee)).Returns(employee);

            //Act
            var result = _employeeController.Delete(employeeId);

            //Assert
            var notFoundObjectResult = result as NotFoundObjectResult;
            Assert.NotNull(notFoundObjectResult);
            Assert.Equal(notFoundObjectResult.StatusCode, (int)HttpStatusCode.NotFound);

            var response = (int)notFoundObjectResult.Value;
            Assert.Equal(employeeId, response);
        }

        [Fact]
        public void Employee_CreateEmployee()
        {
            //setup
            var employee = new Employee()
            {
                Id = 3,
                Name = "Stoyan",
                ClientTable = new List<int>(),
                Type = Models.Enums.EmployeeType.Bartender
            };

            _employeeRepository.Setup(x => x.GetAll()).Returns(Employees);

            _employeeRepository.Setup(x => x.Create(It.IsAny<Employee>())).Callback(() =>
            {
                Employees.Add(employee);
            }).Returns(employee);

            //Act
            var result = _employeeController.CreateEmployee(_mapper.Map<EmployeeRequest>(employee));

            //Assert
            var okObjectResult = result as OkObjectResult;

            Assert.Equal(okObjectResult.StatusCode, (int)HttpStatusCode.OK);
            Assert.NotNull(Employees.FirstOrDefault(x => x.Id == employee.Id));
            Assert.Equal(3, Employees.Count);

        }


    }
}
