import { sequelize, app } from "../instances/sequelize";
import * as Sequelize from "sequelize";
import Roles from "../Entities/Roles.Model";

export class RolesService {
  contexttable: any;
  Roles: Sequelize.Model<{}, {}>;
  constructor() {
    this.Roles = Roles;
  }

  public GetAllRoless = async (req: any, res: any) => {
    await this.Roles.findAll().then(data => {
      return res.json(data);
    });
  };
}
