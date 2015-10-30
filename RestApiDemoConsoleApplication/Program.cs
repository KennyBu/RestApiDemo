using System;
using System.Reflection;
using Ninject;
using RestApiDemoRepository;
using RestSharp;

namespace RestApiDemoConsoleApplication
{
    class Program
    {
        private static ITaskApiRepository _taskRepository;
        private static IRestClient _restClient;

        static void Main()
        {
            _restClient = GetRestClient();

            TestGet();
            TestGetById(123);
            TestPost();

            Console.ReadKey();
        }

        private static void TestPost()
        {
            try 
            {
                _taskRepository = new TaskApiRepository(_restClient);
           
                var task = new Task
                {
                    Id = 123, 
                    Done = false, 
                    Title = "test for post", 
                    Description = "Test for post xxx"
                };

                var response = _taskRepository.PostTask(task);

                Console.WriteLine("Response Status: {0}", response);
            }
            catch (Exception exception)
            {
                DisplayException(exception);
            }            
        }

        private static void TestGet()
        {
            try 
            {
                _taskRepository = new TaskApiRepository(_restClient);

                var tasks = _taskRepository.GetTasks();

                foreach (var task in tasks)
                {
                    Console.WriteLine("\nId: {0}\nTitle: {1}\nDescription: {2}\nDone: {3}", task.Id, task.Title, task.Description, task.Done);
                }
            }
            catch (Exception exception)
            {
                DisplayException(exception);
            }
        }

        private static void TestGetById(int id)
        {
            try
            {
                _taskRepository = new TaskApiRepository(_restClient);

                var task = _taskRepository.GetTaskById(id);

                if (task != null)
                {
                    Console.WriteLine("\nId: {0}\nTitle: {1}\nDescription: {2}\nDone: {3}", task.Id, task.Title, task.Description, task.Done);
                }
                else
                {
                    Console.WriteLine("Task not found.");

                }
            }
            catch (Exception exception)
            {
                DisplayException(exception);
            }
        }

        private static IRestClient GetRestClient()
        {
            IKernel kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());

            return kernel.Get<IRestClient>();
        }

        private static void DisplayException(Exception exception)
        {
            Console.WriteLine("Exception: {0}", exception);
        }
    }
}
