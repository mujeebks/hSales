import { sequelize, app } from "../instances/sequelize";
import * as Sequelize from "sequelize";


import { config } from "../constants/dev.config";
// import { ErrorService } from "./Error.Services";
import { error } from "util";


// All models, can put in or extend to a db object at server init
import * as dbTables from '../Entities/db.tables';
const tables = dbTables.getModels(sequelize); //:dbTables.ITable
//tables.tblemployees.findAll
// Single models
// import * as dbDef from '../Entity/db.d';
// const Employees:dbDef.tblemployeesModel = sequelize.import('../Entity/tblemployees.ts');
// Employees.findAll


export class CategoryService {
  //   Employee: Sequelize.Model<{}, {}>;
  // ErrorServiceModel = new ErrorService();
  Employees:Sequelize.Sequelize;
  
  
  constructor() {

  
    // tables.tblemployees.belongsTo(tables.tblroles, { foreignKey: "RoleId" });
    // tables.tblemployees.belongsTo(tables.tbldepartment, { foreignKey: "departmentid" });
    // tables.tblemployees.hasMany(tables.tblitems, {
    //   foreignKey: "employeeid",
    //   sourceKey: "EmployeeId"
    // });
  }

  public GetAllCategories = async (req: any, res: any, next: any) => {
  debugger
    let { id } = req.params;
    let wherecondition: any = {};
    if (id) {
      id = parseInt(id);
      wherecondition = {
        EmployeeId: id
      };
    }
    try {
      await tables.mtrCategory.findAll({
        // attributes:['EmployeeName'],
        // include: [
        //   { model: tables.tblroles },
        //   { model: tables.tbldepartment },
        //   { model: tables.tblitems, attributes: ["itemname"] }
        // ],
       // limit: 10,
        logging: config.logging,
        where: wherecondition,
        order: [["category", "DESC"]]
      }).then(datafromdb => {
        return res.json(datafromdb);
      });
    } catch (e) {
     // this.ErrorServiceModel.CreateErrorLog(req, res, next, e);

      return res.json(e.message);
    }
  };

  public DeleteCategory = async (req: any, res: any, next: any) => {
    let { id } = req.params;

    if (id) {
      id = parseInt(id);
    }
    try {

     
      tables.mtrCategory.destroy({
        logging: config.logging,
        where: { categoryid: id }
      }).then(data => {
        return res.json("Record Deleted");
      });
    } catch (e) {
     // this.ErrorServiceModel.CreateErrorLog(req, res, next, e);
      return res.json(e.message);
    }
  };

  public SaveCategories = async (req: any, res: any, next: any) => {
    debugger
    let { id } = req.params;

    if (id) {
      id = parseInt(id);
    }
    try {
      let Model = req.body
      if (!id) {
        await tables.mtrCategory.create(
      
          Model,
          { logging: config.logging},
          
        )
          .then(data => {
          return res.json("Created");
        });
      } else {
        //update
        await tables.mtrCategory.update(Model, {
          logging: config.logging,
          where: { categoryid: id }
          
        }).then(data => {
          return res.json("Updated");
        });
      }
    } catch (e) {
     // this.ErrorServiceModel.CreateErrorLog(req, res, next, e);
      return res.json(e.message);
    }
  };
}
