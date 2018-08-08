/* jshint indent: 2 */
// tslint:disable
import * as sequelize from 'sequelize';
import {DataTypes} from 'sequelize';
import {mtrAreaInstance, mtrAreaAttribute} from './db';

module.exports = function(sequelize: sequelize.Sequelize, DataTypes: DataTypes) {
  return sequelize.define<mtrAreaInstance, mtrAreaAttribute>('mtrArea', {
    areaID: {
      type: DataTypes.BIGINT,
      allowNull: false,
      defaultValue: '((0))',
      primaryKey: true
    },
    areaName: {
      type: DataTypes.STRING,
      allowNull: true
    },
    deactive: {
      type: DataTypes.BOOLEAN,
      allowNull: true,
      defaultValue: '0'
    },
    SSMA_TimeStamp: {
      type: DataTypes.DATE,
      allowNull: false
    }
  }, {
    tableName: 'mtrArea'
  });
};
