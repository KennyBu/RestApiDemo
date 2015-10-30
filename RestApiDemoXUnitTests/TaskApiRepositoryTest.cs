using System;
using System.Net;
using Moq;
using RestApiDemoRepository;
using RestSharp;
using Shouldly;
using Xunit;

namespace RestApiDemoXUnitTests
{
    public class TaskApiRepositoryTest
    {
        private readonly Mock<IRestClient> _mockedRestClient = new Mock<IRestClient>();

        [Fact]
        public void PostTaskTest_VerifyResponseNotNull()
        {
            IRestResponse<TaskList> restResponse = new RestResponse<TaskList>();

            _mockedRestClient.Setup(x => x.Execute<TaskList>(It.IsAny<IRestRequest>())).Returns(restResponse);
            var taskApiRepository = new TaskApiRepository(_mockedRestClient.Object);

            var task = TestHelper.GetTestTask();

            var response = taskApiRepository.PostTask(task);

            response.ShouldNotBeNull();
        }

        [Fact]
        public void PostTaskTest_ResponseNull_VerifyExceptionThrown()
        {
            IRestResponse<TaskList> restResponse = null;

            _mockedRestClient.Setup(x => x.Execute<TaskList>(It.IsAny<IRestRequest>())).Returns(restResponse);
            var taskApiRepository = new TaskApiRepository(_mockedRestClient.Object);

            var task = TestHelper.GetTestTask();

            Should.Throw<Exception>
            (
                () => taskApiRepository.PostTask(task)
            );
        }

        [Fact]
        public void PostTaskTest__VerifyReturnsResponseStatusCodeIsCreated()
        {
            const HttpStatusCode expectedStatusCode = HttpStatusCode.Created;

            IRestResponse<TaskList> restResponse = new RestResponse<TaskList>();
            restResponse.StatusCode = HttpStatusCode.Created;

            _mockedRestClient.Setup(x => x.Execute<TaskList>(It.IsAny<IRestRequest>())).Returns(restResponse);

            var taskApiRepository = new TaskApiRepository(_mockedRestClient.Object);

            var task = TestHelper.GetTestTask();

            var actualStatusCode = taskApiRepository.PostTask(task);
           
           expectedStatusCode.ShouldBe(actualStatusCode);
        }

        [Fact]
        public void PostTaskTest__VerifyExecuteCalledOnce()
        {
            IRestResponse<TaskList> restResponse = new RestResponse<TaskList>();

            _mockedRestClient.Setup(x => x.Execute<TaskList>(It.IsAny<IRestRequest>())).Returns(restResponse);

            var taskApiRepository = new TaskApiRepository(_mockedRestClient.Object);

            var task = TestHelper.GetTestTask();

            var response = taskApiRepository.PostTask(task);

            _mockedRestClient.Verify(x => x.Execute<TaskList>(It.IsAny<IRestRequest>()), Times.Once());
        }

        [Fact]
        public void GetTasksTest_VerifyTasksNotNull()
        {
            IRestResponse<TaskList> restResponse = new RestResponse<TaskList>();

            _mockedRestClient.Setup(x => x.Execute<TaskList>(It.IsAny<IRestRequest>())).Returns(restResponse);

            var taskApiRepository = new TaskApiRepository(_mockedRestClient.Object);

            var taskList = taskApiRepository.GetTasks();

            taskList.ShouldNotBeNull();
        }

        [Fact]
        public void GetTasksTest_VerifyExecuteCalledOnce()
        {
            var taskApiRepository = new TaskApiRepository(_mockedRestClient.Object);

            var taskList = taskApiRepository.GetTasks();

            _mockedRestClient.Verify(x => x.Execute<TaskList>(It.IsAny<IRestRequest>()), Times.Once());
        }

        [Fact]
        public void GetTasksTest_Response_Null_VerifyTaskListCountIsZero()
        {
            IRestResponse<TaskList> restResponse = null;

            _mockedRestClient.Setup(x => x.Execute<TaskList>(It.IsAny<IRestRequest>())).Returns(restResponse);

            var taskApiRepository = new TaskApiRepository(_mockedRestClient.Object);

            var taskList = taskApiRepository.GetTasks();

            taskList.Count.ShouldBe(0);
        }

        [Fact]
        public void GetByIdTest_Id_123_VerifyTaskNotNull()
        {
            var result = TestHelper.GetTestSingleTaskResult();
            _mockedRestClient.Setup(x => x.Execute<SingleTaskResult>(It.IsAny<IRestRequest>()).Data).Returns(result);
            var taskApiRepository = new TaskApiRepository(_mockedRestClient.Object);

            const int id = 123;

            var task = taskApiRepository.GetTaskById(id);

            task.ShouldNotBeNull();
        }

        [Fact]
        public void GetByIdTest_Id_123_Returns_Null_VerifyTaskIsNull()
        {
            SingleTaskResult result = null;
            _mockedRestClient.Setup(x => x.Execute<SingleTaskResult>(It.IsAny<IRestRequest>()).Data).Returns(result);
            var taskApiRepository = new TaskApiRepository(_mockedRestClient.Object);

            const int id = 123;

            var task = taskApiRepository.GetTaskById(id);

            task.ShouldBeNull();
        }

        [Fact]
        public void GetByIdTest_Id_123_VerifyExecuteCalledOnce()
        {
            var result = TestHelper.GetTestSingleTaskResult();
            _mockedRestClient.Setup(x => x.Execute<SingleTaskResult>(It.IsAny<IRestRequest>()).Data).Returns(result);
            var taskApiRepository = new TaskApiRepository(_mockedRestClient.Object);

            const int id = 123;

            var task = taskApiRepository.GetTaskById(id);

            _mockedRestClient.Verify(x => x.Execute<SingleTaskResult>(It.IsAny<IRestRequest>()).Data, Times.Once());
        }
    }
}