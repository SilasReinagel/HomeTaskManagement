
class ApiUrls {
    static login() {
        return "https://miniauth.azurewebsites.net/api/account/applogin";
    }

    static createTask() {
        return "http://localhost:8588/api/command/createtask";
    }
}

export default ApiUrls;