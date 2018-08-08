// tslint:disable
import * as Sequelize from 'sequelize';


// table: tblitems
export interface tblitemsAttribute {
  itemid:number;
  itemname?:string;
  employeeid?:number;
}
export interface tblitemsInstance extends Sequelize.Instance<tblitemsAttribute>, tblitemsAttribute { }
export interface tblitemsModel extends Sequelize.Model<tblitemsInstance, tblitemsAttribute> { }

// table: tblemployees
export interface tblemployeesAttribute {
  EmployeeId:number;
  RoleId?:number;
  EmployeeName?:string;
  age?:number;
  departmentid?:number;
}
export interface tblemployeesInstance extends Sequelize.Instance<tblemployeesAttribute>, tblemployeesAttribute { }
export interface tblemployeesModel extends Sequelize.Model<tblemployeesInstance, tblemployeesAttribute> { }

// table: users
export interface usersAttribute {
  id:number;
  firstname?:string;
  lastname?:string;
  age?:number;
  createdAt:Date;
  updatedAt:Date;
}
export interface usersInstance extends Sequelize.Instance<usersAttribute>, usersAttribute { }
export interface usersModel extends Sequelize.Model<usersInstance, usersAttribute> { }

// table: tblroles
export interface tblrolesAttribute {
  RoleId:number;
  RoleDescription?:string;
}
export interface tblrolesInstance extends Sequelize.Instance<tblrolesAttribute>, tblrolesAttribute { }
export interface tblrolesModel extends Sequelize.Model<tblrolesInstance, tblrolesAttribute> { }

// table: tblipaddress
export interface tblipaddressAttribute {
  id:number;
  ipaddress:string;
  name?:string;
}
export interface tblipaddressInstance extends Sequelize.Instance<tblipaddressAttribute>, tblipaddressAttribute { }
export interface tblipaddressModel extends Sequelize.Model<tblipaddressInstance, tblipaddressAttribute> { }

// table: tbldepartment
export interface tbldepartmentAttribute {
  departmentid:number;
  departmentname?:string;
}
export interface tbldepartmentInstance extends Sequelize.Instance<tbldepartmentAttribute>, tbldepartmentAttribute { }
export interface tbldepartmentModel extends Sequelize.Model<tbldepartmentInstance, tbldepartmentAttribute> { }

// table: Errors
export interface ErrorsAttribute {
  ErrorId:number;
  IpAddress?:string;
  Date?:Date;
  Message?:string;
  StackTrace?:string;
}
export interface ErrorsInstance extends Sequelize.Instance<ErrorsAttribute>, ErrorsAttribute { }
export interface ErrorsModel extends Sequelize.Model<ErrorsInstance, ErrorsAttribute> { }
