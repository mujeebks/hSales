/* jshint indent: 2 */
// tslint:disable
import * as sequelize from 'sequelize';
import {DataTypes} from 'sequelize';
import {mtrRoleInstance, mtrRoleAttribute} from './db';

module.exports = function(sequelize: sequelize.Sequelize, DataTypes: DataTypes) {
  return sequelize.define<mtrRoleInstance, mtrRoleAttribute>('mtrRole', {
    roleId: {
      type: DataTypes.INTEGER,
      allowNull: false,
      primaryKey: true
    },
    roleName: {
      type: DataTypes.STRING,
      allowNull: true
    },
    deActive: {
      type: DataTypes.BOOLEAN,
      allowNull: true,
      defaultValue: '0'
    },
    SSMA_TimeStamp: {
      type: DataTypes.DATE,
      allowNull: false
    }
  }, {
    tableName: 'mtrRole'
  });
};
