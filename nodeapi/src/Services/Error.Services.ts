import Errors from "../Entities/Error.Model";

import { sequelize } from "../instances/sequelize";
import { config } from "../constants/dev.config";
import IpAddresss from "../Entities/IpAddress.Model";

export class ErrorService {
  constructor() {
    Errors.belongsTo(IpAddresss, {
      foreignKey: "IpAddress",
      targetKey: "ipaddress"
    });
  }

  public CreateErrorLog = async (req, res, next, ex) => {
    const existingUserCredentials = {
      ErrorId: 0,
      IpAddress: req.header("x-forwarded-for") || req.connection.remoteAddress,
      Date: new Date(),
      Message: ex.message.toString(),
      StackTrace: ex.stack
        .split("\n")
        .slice(0, 2)
        .join("\n")
    };
    console.log(JSON.stringify(existingUserCredentials));
    try {
      //  await Errors.sync({force:true});
      await Errors.create(existingUserCredentials);
    } catch (err) {
      throw err;
    }
  };

  public GetErrorLogs = async (req, res, next) => {
    try {
      await Errors.findAll({
        include: [{ model: IpAddresss }]
      }).then(datafromdb => {
        return res.json(datafromdb);
      });
    } catch (e) {
      return res.json(e.message);
    }
  };
}
