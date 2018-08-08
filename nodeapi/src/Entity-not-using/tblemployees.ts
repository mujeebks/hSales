/* jshint indent: 2 */
// tslint:disable
import * as sequelize from 'sequelize';
import {DataTypes} from 'sequelize';
import {tblemployeesInstance, tblemployeesAttribute} from './db';

module.exports = function(sequelize: sequelize.Sequelize, DataTypes: DataTypes) {
  return sequelize.define<tblemployeesInstance, tblemployeesAttribute>('tblemployees', {
    EmployeeId: {
      type: DataTypes.INTEGER,
      allowNull: false,
      primaryKey: true,
      autoIncrement: true
    },
    RoleId: {
      type: DataTypes.INTEGER,
      allowNull: true
    },
    EmployeeName: {
      type: DataTypes.STRING,
      allowNull: true
    },
    age: {
      type: DataTypes.INTEGER,
      allowNull: true
    },
    departmentid: {
      type: DataTypes.INTEGER,
      allowNull: true,
      references: {
        model: 'tbldepartment',
        key: 'departmentid'
      }
    }
  }, {
    tableName: 'tblemployees',
    timestamps: false
  });
};
