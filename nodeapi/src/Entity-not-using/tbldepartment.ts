/* jshint indent: 2 */
// tslint:disable
import * as sequelize from 'sequelize';
import {DataTypes} from 'sequelize';
import {tbldepartmentInstance, tbldepartmentAttribute} from './db';

module.exports = function(sequelize: sequelize.Sequelize, DataTypes: DataTypes) {
  return sequelize.define<tbldepartmentInstance, tbldepartmentAttribute>('tbldepartment', {
    departmentid: {
      type: DataTypes.INTEGER,
      allowNull: false,
      primaryKey: true,
      autoIncrement: true
    },
    departmentname: {
      type: DataTypes.STRING,
      allowNull: true
    }
  }, {
    tableName: 'tbldepartment',
    timestamps: false
  });
};
