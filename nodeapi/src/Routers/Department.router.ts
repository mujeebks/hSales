import * as express from "express";
import { DepartmentService } from "../Services/Department.services";
import { app } from "../instances/sequelize";


var DepartmentServiceModel = new DepartmentService();
const DepartmentRouter = express.Router();

DepartmentRouter.get("/:id", function(req, res, next) {
     DepartmentServiceModel.GetDepartments(req, res, next);
});

DepartmentRouter.get("/", function(req, res, next) {
    DepartmentServiceModel.GetDepartments(req, res, next);
});

DepartmentRouter.post("/", function(req, res, next) {
    DepartmentServiceModel.SaveDepartments(req, res, next);
});

DepartmentRouter.put("/:id", function(req, res, next) {
    DepartmentServiceModel.SaveDepartments(req, res, next);
});

DepartmentRouter.delete("/:id", function(req, res, next) {
        DepartmentServiceModel.DeleteDepartments(req, res, next);
  
});


export default DepartmentRouter;
