using System;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestApiDemoRepository;
using RestSharp;

namespace RestApiDemoUnitTests
{
    [TestClass]
    public class TaskApiRepositoryTest
    {
        private readonly Mock<IRestClient> _mockedRestClient = new Mock<IRestClient>();
        
        [TestMethod]
        public void PostTaskTest_VerifyResponseNotNull()
        {
            IRestResponse<TaskList> restResponse = new RestResponse<TaskList>();

            _mockedRestClient.Setup(x => x.Execute<TaskList>(It.IsAny<IRestRequest>())).Returns(restResponse);
            var taskApiRepository = new TaskApiRepository(_mockedRestClient.Object);

            var task = GetTestTask();

            var response = taskApiRepository.PostTask(task);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void PostTaskTest_ResponseNull_VerifyExceptionThrown()
        {
            IRestResponse<TaskList> restResponse = null;
            var exceptionThrown = false;

            _mockedRestClient.Setup(x => x.Execute<TaskList>(It.IsAny<IRestRequest>())).Returns(restResponse);
            var taskApiRepository = new TaskApiRepository(_mockedRestClient.Object);

            var task = GetTestTask();

            try
            { 
                var response = taskApiRepository.PostTask(task);
            }
            catch (Exception)
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod]
        public void PostTaskTest__VerifyReturnsResponseStatusCodeIsCreated()
        {
            const HttpStatusCode expectedStatusCode = HttpStatusCode.Created;

            IRestResponse<TaskList> restResponse = new RestResponse<TaskList>();
            restResponse.StatusCode = HttpStatusCode.Created;

            _mockedRestClient.Setup(x => x.Execute<TaskList>(It.IsAny<IRestRequest>())).Returns(restResponse);

            var taskApiRepository = new TaskApiRepository(_mockedRestClient.Object);

            var task = GetTestTask();

            var actualStatusCode = taskApiRepository.PostTask(task);
            
            Assert.AreEqual(expectedStatusCode, actualStatusCode);
        }

        [TestMethod]
        public void PostTaskTest__VerifyExecuteCalledOnce()
        {
            IRestResponse<TaskList> restResponse = new RestResponse<TaskList>();

            _mockedRestClient.Setup(x => x.Execute<TaskList>(It.IsAny<IRestRequest>())).Returns(restResponse);

            var taskApiRepository = new TaskApiRepository(_mockedRestClient.Object);

            var task = GetTestTask();

            var response = taskApiRepository.PostTask(task);

            _mockedRestClient.Verify(x => x.Execute<TaskList>(It.IsAny<IRestRequest>()), Times.Once());
        }
        
        [TestMethod]
        public void GetTasksTest_VerifyTasksNotNull()
        {
            IRestResponse<TaskList> restResponse = new RestResponse<TaskList>();

            _mockedRestClient.Setup(x => x.Execute<TaskList>(It.IsAny<IRestRequest>())).Returns(restResponse);

            var taskApiRepository = new TaskApiRepository(_mockedRestClient.Object);

            var taskList = taskApiRepository.GetTasks();

            Assert.IsNotNull(taskList);
        }

        [TestMethod]
        public void GetTasksTest_VerifyExecuteCalledOnce()
        {
            var taskApiRepository = new TaskApiRepository(_mockedRestClient.Object);

            var taskList = taskApiRepository.GetTasks();

            _mockedRestClient.Verify(x => x.Execute<TaskList>(It.IsAny<IRestRequest>()), Times.Once());
        }

        [TestMethod]
        public void GetTasksTest_Response_Null_VerifyTaskListCountIsZero()
        {
            IRestResponse<TaskList> restResponse = null;

            _mockedRestClient.Setup(x => x.Execute<TaskList>(It.IsAny<IRestRequest>())).Returns(restResponse);

            var taskApiRepository = new TaskApiRepository(_mockedRestClient.Object);

            var taskList = taskApiRepository.GetTasks();

            Assert.IsTrue(taskList.Count == 0);
        }
        
        [TestMethod]
        public void GetByIdTest_Id_123_VerifyTaskNotNull()
        {
            var result = GetTestSingleTaskResult();
            _mockedRestClient.Setup(x => x.Execute<SingleTaskResult>(It.IsAny<IRestRequest>()).Data).Returns(result);
            var taskApiRepository = new TaskApiRepository(_mockedRestClient.Object);

            const int id = 123;

            var task = taskApiRepository.GetTaskById(id);

            Assert.IsNotNull(task);
        }

        [TestMethod]
        public void GetByIdTest_Id_123_Returns_Null_VerifyTaskIsNull()
        {
            SingleTaskResult result = null;
            _mockedRestClient.Setup(x => x.Execute<SingleTaskResult>(It.IsAny<IRestRequest>()).Data).Returns(result);
            var taskApiRepository = new TaskApiRepository(_mockedRestClient.Object);

            const int id = 123;

            var task = taskApiRepository.GetTaskById(id);

            Assert.IsNull(task);
        }

        [TestMethod]
        public void GetByIdTest_Id_123_VerifyExecuteCalledOnce()
        {
            var result = GetTestSingleTaskResult();
            _mockedRestClient.Setup(x => x.Execute<SingleTaskResult>(It.IsAny<IRestRequest>()).Data).Returns(result);
            var taskApiRepository = new TaskApiRepository(_mockedRestClient.Object);

            const int id = 123;

            var task = taskApiRepository.GetTaskById(id);

            _mockedRestClient.Verify(x => x.Execute<SingleTaskResult>(It.IsAny<IRestRequest>()).Data, Times.Once());
        }
        
        private Task GetTestTask()
        {
            return new Task { Id = 123, Done = false, Title = "Test Title for POST", Description = "Test Description for POST" };
        }

        private SingleTaskResult GetTestSingleTaskResult()
        {
            var taskResult = new SingleTaskResult
            {
                Task = new Task {Id = 123, Done = false, Title = "Test Title", Description = "Test Description"}
            };

            return taskResult; 
        }
    }
}
