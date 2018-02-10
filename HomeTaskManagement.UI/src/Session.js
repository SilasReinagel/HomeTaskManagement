
class Session {
  getUnixUtcTimeSeconds() {    
    var tmLoc = new Date();
    return Math.floor((tmLoc.getTime() + (tmLoc.getTimezoneOffset() * 60000)) / 1000);
  }

  setLoginInfo(username, token, expiration) {
    localStorage.setItem("taskAppUsername", username);
    localStorage.setItem("miniAuthToken", token);
    localStorage.setItem("miniAuthTokenExpiration", expiration);
  }

  username() {
    return localStorage.getItem("taskAppUsername");
  }

  token() {
    return localStorage.getItem("miniAuthToken");
  }

  logout() {
    localStorage.clear();
  }

  isLoggedIn() {
    var now = this.getUnixUtcTimeSeconds();
    var tokenExpiration = localStorage.getItem("miniAuthTokenExpiration");
    var isLoggedIn = !!tokenExpiration && tokenExpiration < now;
    console.log('Is Logged In: ' + isLoggedIn);
    return isLoggedIn;
  }
}

export default Session;
