using static System.Net.WebRequestMethods;

namespace NationalParkWeb
{
    public static class StaticDetails     //https://localhost:7186/api/v1/nationalParks
    {
        public static string APIBaseUrl = "https://localhost:7186/";
        public static string NationalParkAPIPathGetAll= APIBaseUrl + "api/v1/nationalParks/GetNationalParks/";
        public static string NationalParkAPIPathById = APIBaseUrl + "api/v1/nationalParks/";
        public static string NationalParkAPIPathByIdDelete = APIBaseUrl + "api/v1/nationalParks/DeleteNationalPark/";
        public static string NationalParkAPIPathByIdCreate = APIBaseUrl + "api/v1/nationalParks/CreateNationalPark/";
        public static string TrailAPIPath = APIBaseUrl + "api/v1/trails/";
        public static string TrailAPIPathCreate = APIBaseUrl + "api/v1/trails/CreateTrail";
        public static string TrailAPIPathDelete = APIBaseUrl + "api/v1/trails/DeleteTrail";
        public static string AccountAPIPath = APIBaseUrl + "api/v1/users/";
    }
}
