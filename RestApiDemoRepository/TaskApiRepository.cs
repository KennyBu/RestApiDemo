using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using RestSharp;

namespace RestApiDemoRepository
{
    public class TaskApiRepository : ITaskApiRepository
    {
        private readonly IRestClient _restClient;

        private const string TaskApiBasePath = "api/tasks";

        public TaskApiRepository(IRestClient restClient)
        {            
            _restClient = restClient;
        }

        public HttpStatusCode PostTask(Task task)
        {
            IRestRequest request = new RestRequest(TaskApiBasePath, Method.POST);

            request.JsonSerializer = new JsonSerializer();
            request.RequestFormat = DataFormat.Json;
            request.AddBody(task);

            var response = _restClient.Execute<TaskList>(request);

            if (response == null)
            {
                throw new Exception("Response is null");
            }

            return response.StatusCode;            
        }

        public List<Task> GetTasks()
        {
            IRestRequest request = new RestRequest(TaskApiBasePath, Method.GET);
            var response = _restClient.Execute<TaskList>(request);
            var taskList = new List<Task>();

            if (response != null && response.Data != null)
            {
                taskList = response.Data.Tasks;
            }

            return taskList;
        }

        public Task GetTaskById(int id)
        {
            const string taskApiPath = TaskApiBasePath + "/{id}";
            IRestRequest request = new RestRequest(taskApiPath, Method.GET);

            request.AddParameter("id", id, ParameterType.UrlSegment);
            var response = _restClient.Execute<SingleTaskResult>(request).Data;
            var task = response != null ? response.Task : null;
            
            return task;
        }
    }
}
