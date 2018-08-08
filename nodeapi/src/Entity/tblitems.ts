/* jshint indent: 2 */
// tslint:disable
import * as sequelize from 'sequelize';
import {DataTypes} from 'sequelize';
import {tblitemsInstance, tblitemsAttribute} from './db';

module.exports = function(sequelize: sequelize.Sequelize, DataTypes: DataTypes) {
  return sequelize.define<tblitemsInstance, tblitemsAttribute>('tblitems', {
    itemid: {
      type: DataTypes.INTEGER,
      allowNull: false,
      primaryKey: true,
      autoIncrement: true
    },
    itemname: {
      type: DataTypes.STRING,
      allowNull: true
    },
    employeeid: {
      type: DataTypes.INTEGER,
      allowNull: true
    }
  }, {
    tableName: 'tblitems',
    timestamps: false
  });
};
