using RestApiDemoRepository;

namespace RestApiDemoXUnitTests
{
    public static class TestHelper
    {
        public static Task GetTestTask()
        {
            return new Task { Id = 123, Done = false, Title = "Test Title for POST", Description = "Test Description for POST" };
        }

        public static SingleTaskResult GetTestSingleTaskResult()
        {
            var taskResult = new SingleTaskResult
            {
                Task = new Task { Id = 123, Done = false, Title = "Test Title", Description = "Test Description" }
            };

            return taskResult;
        }
    }
}