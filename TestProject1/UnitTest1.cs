using AddressBookRestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;



namespace TestProject1
{
    [TestClass]
    public class RestSharpTestCase
    {
        RestClient client;

        [TestInitialize]
        public void Setup()
        {
            client = new RestClient("http://localhost:3000");
        }
        private RestResponse getEmployeeList()
        {
            RestRequest request = new RestRequest("/Persons", Method.Get);

            //act

            RestResponse response = (RestResponse)client.Execute(request);
            return response;
        }

        [TestMethod]
        public void onCallingGETApi_ReturnEmployeeList()
        {
            RestResponse response = getEmployeeList();

            //assert
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
            List<PersonModel> dataResponse = JsonConvert.DeserializeObject<List<PersonModel>>(response.Content);
            Assert.AreEqual(5, dataResponse.Count);
            foreach (var item in dataResponse)
            {
                System.Console.WriteLine("id: " + item.Id + " First Name: " + item.First_Name + " Last Name: " + item.Last_Name);
            }
        }


        [TestMethod]
        public void givenEmployee_OnPost_ShouldReturnAddedEmployee()
        {
            RestRequest request = new RestRequest("/Persons", Method.Post);
            JObject jObjectbody = new JObject();
            jObjectbody.Add("First_Name", "Mrunal");
            jObjectbody.Add("Last_Name", "Joshi");
            jObjectbody.Add("Address", "Garkheda");
            jObjectbody.Add("City", "Aurangabad");
            jObjectbody.Add("State", "Maharashtra");
            jObjectbody.Add("Zip", "431001");
            jObjectbody.Add("Email", "mrunaljoshi@gmail.com");
            jObjectbody.Add("Phone_Num", "7048642874");

            request.AddParameter("application/json", jObjectbody, ParameterType.RequestBody);

            //act
            RestResponse response = (RestResponse)client.Execute(request);
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.Created);
            PersonModel dataResponse = JsonConvert.DeserializeObject<PersonModel>(response.Content);
            Assert.AreEqual("Mrunal", dataResponse.First_Name);
            Assert.AreEqual("Joshi", dataResponse.Last_Name);
            Assert.AreEqual("Garkheda", dataResponse.Address);
            Assert.AreEqual("Aurangabad", dataResponse.City);
            Assert.AreEqual("Maharashtra", dataResponse.State);
            Assert.AreEqual(431001, dataResponse.Zip);
            Assert.AreEqual("mrunaljoshi@gmail.com", dataResponse.Email);
            Assert.AreEqual("7048642874", dataResponse.Phone_Number);

        }
    }
}

