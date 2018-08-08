import { sequelize, app } from "../instances/sequelize";
import * as Sequelize from "sequelize";
import User from "../Entities/user.model";

export class UserService {
  contexttable: any;
  user: Sequelize.Model<{}, {}>;
  constructor() {
    this.user = User;
  }

  public CreateTable() {
    this.user.sync().then(() => {
      this.user
        .create({
          firstname: "riyas",
          lastname: "ks",
          age: 29
        })
        .then(data => {});
    });
  }

  public async InsertUser(model: any, res, req) {
    try {
      await this.user.create(model).then(() => {
        console.log("new user " + model.firstname + "is created");
        return res.json("new user " + model.firstname + "is created");
      });
    } catch (e) {
      return res.json(e.message);
    }
  }

  public GetAllUsers = async (req: any, res: any) => {
    await this.user.findAll().then(data => {
      return res.json(data);
    });
  };

  public GetDash: any = async (res: any, req: Request) => {
    sequelize.query("BI_GetAllItems").then(data => {
      return res.json("hffh");
    });
  };
}
