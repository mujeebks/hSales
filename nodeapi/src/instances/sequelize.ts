import * as Sequelize from "sequelize";
var cors = require('cors')
import * as express from "express";

var sequelize = new Sequelize("Sequilize", "sa", "abc123*", {
  dialect: "mssql",
  host: "192.168.10.106",
  username: "sa",
  password: "abc123*",
  dialectOptions: {
    port: 8016
  },
  define: {
    timestamps: false  // I don't want timestamp fields by default
  },
});

// var config:any = {
//   "user": 'sa',
//   "password": 'abc123*',
//   "server": 'RIYAS-KS',
//   "database": 'XeniaPOSVLite',
//   "port": '61427',
//   "dialect": "mssql",
//   "dialectOptions": {
//       "instanceName": "MSSQLSERVER"
//   }
// };
// var sequelize = new Sequelize(config)
sequelize
  .authenticate()
  .then(() => {
    console.log("Connection has been established successfully.API running....");
  })
  .catch(err => {
    console.error("Unable to connect to the database:", err);
  });
var app = express();
app.use(cors())
const port = 4001;
app.listen(port, "192.168.10.165", () => {
  console.log(`server is listening on port ${port}`);
});

export { sequelize, app };
