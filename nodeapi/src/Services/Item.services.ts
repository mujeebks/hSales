import { sequelize, app } from "../instances/sequelize";
import * as Sequelize from "sequelize";
import Employees from "../Entities/Employees.Model";

import Items from "../Entities/Items.Model";
import { config } from "../constants/dev.config";
import { ErrorService } from "./Error.Services";
import { IItemsInstance } from "../Entities/Items.Model";

export class ItemService {
  contexttable: any;
  ErrorServiceModel = new ErrorService();
  constructor() {
    Employees.hasMany(Items, {
      foreignKey: "employeeid",
      sourceKey: "EmployeeId"
    });

    Items.belongsTo(Employees, {
      foreignKey: "employeeid"
    });
  }

  public GetAllItems = async (req: any, res: any, next: any) => {
    let { id } = req.params;
    let wherecondition: any = {};
    if (id) {
      id = parseInt(id);
      wherecondition = {
        EmployeeId: id
      };
    }
    try {
      await Items.findAll({
        attributes: ["itemname"],

        include: [{ model: Employees, attributes: ["EmployeeName"] }],

        // limit: 10,
        logging: config.logging,
        where: wherecondition,
        order: [["itemname", "DESC"]]
      }).then(datafromdb => {
        return res.json(datafromdb);
      });
    } catch (e) {
      this.ErrorServiceModel.CreateErrorLog(req, res, next, e);
      return res.json(e.message);
    }
  };

  public GetItemsByStoredPrcedure = async (req: any, res: any, next: any) => {
    debugger;
    try {
      await sequelize.query("sp_GetItems").then(data => {
        return res.json(data);
      });
    } catch (e) {
      this.ErrorServiceModel.CreateErrorLog(req, res, next, e);
      return res.json(e.message);
    }
  };
}
