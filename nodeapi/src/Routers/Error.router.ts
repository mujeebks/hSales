import * as express from "express";
import { ErrorService } from "../Services/Error.Services";

var ErrorService_Service = new ErrorService();
const ErrorRouter = express.Router();

ErrorRouter.get("/:id", function(req, res, next) {});

ErrorRouter.get("/", function(req, res, next) {
  ErrorService_Service.GetErrorLogs(req, res, next);
});

export default ErrorRouter;
