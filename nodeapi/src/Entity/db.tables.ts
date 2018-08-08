// tslint:disable
import * as path from 'path';
import * as sequelize from 'sequelize';
import * as def from './db';

export interface ITables {
  tblitems:def.tblitemsModel;
  tblemployees:def.tblemployeesModel;
  users:def.usersModel;
  tblroles:def.tblrolesModel;
  tblipaddress:def.tblipaddressModel;
  tbldepartment:def.tbldepartmentModel;
  Errors:def.ErrorsModel;
}

export const getModels = function(seq:sequelize.Sequelize):ITables {
  const tables:ITables = {
    tblitems: seq.import(path.join(__dirname, './tblitems')),
    tblemployees: seq.import(path.join(__dirname, './tblemployees')),
    users: seq.import(path.join(__dirname, './users')),
    tblroles: seq.import(path.join(__dirname, './tblroles')),
    tblipaddress: seq.import(path.join(__dirname, './tblipaddress')),
    tbldepartment: seq.import(path.join(__dirname, './tbldepartment')),
    Errors: seq.import(path.join(__dirname, './Errors')),
  };
  return tables;
};
