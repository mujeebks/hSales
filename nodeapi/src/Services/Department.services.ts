import { sequelize, app } from "../instances/sequelize";
import * as Sequelize from "sequelize";
import { ErrorService } from "./Error.Services";
import Departments from "../Entities/Department.Model";
import { config } from "../constants/dev.config";
import Employees from "../Entities/Employees.Model";

export class DepartmentService {
  ErrorServiceModel = new ErrorService();

  constructor() {
    // Employees.belongsTo(Departments, { foreignKey: "departmentid" });
  }

  public GetDepartments = async (req: any, res: any, next: any) => {
    debugger;
    let { id } = req.params;
    let wherecondition: any = {};
    if (id) {
      id = parseInt(id);
      wherecondition = {
        departmentid: id
      };
    }
    try {
      await Departments.findAll({
        // attributes:['departmentname'],
        logging: config.logging,
        where: wherecondition,
        order: [["departmentname", "DESC"]]
      }).then(datafromdb => {
        return res.json(datafromdb);
      });
    } catch (e) {
      this.ErrorServiceModel.CreateErrorLog(req, res, next, e);
      return res.json(e.message);
    }
  };
  public DeleteDepartments = async (req: any, res: any, next: any) => {
   
    let { id } = req.params;

    if (id) {
      id = parseInt(id);
    }

    Departments.destroy({
      logging: config.logging,
      where: { departmentid: id }
    }).then(
      data => {
        debugger;
        return res.json("Record Deleted");
      },
      err => {
        this.ErrorServiceModel.CreateErrorLog(req, res, next, err);
        return res.json(err.message);
      }
    );
  };

  public SaveDepartments = async (req: any, res: any, next: any) => {
    debugger;
    let { id } = req.params;

    if (id) {
      id = parseInt(id);
    }
    try {
      let Model = req.body;
      if (!id) {
        await Departments.create(Model, { logging: config.logging }).then(
          data => {
            return res.json("Created");
          }
        );
      } else {
        //update
        await Departments.update(Model, {
          logging: config.logging,
          where: { departmentid: id }
        }).then(data => {
          return res.json("Updated");
        });
      }
    } catch (e) {
      this.ErrorServiceModel.CreateErrorLog(req, res, next, e);
      return res.json(e.message);
    }
  };
}
