import axios from 'axios';
import Session from "./Session";

class TaskClient {
  static init(session) {
    this.state = {
      session:session
    }
  } 
}

export default TaskClient;
