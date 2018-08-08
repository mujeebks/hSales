/* jshint indent: 2 */
// tslint:disable
import * as sequelize from 'sequelize';
import {DataTypes} from 'sequelize';
import {trsUserlogInstance, trsUserlogAttribute} from './db';

module.exports = function(sequelize: sequelize.Sequelize, DataTypes: DataTypes) {
  return sequelize.define<trsUserlogInstance, trsUserlogAttribute>('trsUserlog', {
    userId: {
      type: DataTypes.INTEGER,
      allowNull: false
    },
    userName: {
      type: DataTypes.STRING,
      allowNull: true
    },
    screenName: {
      type: DataTypes.STRING,
      allowNull: true
    },
    actionType: {
      type: DataTypes.STRING,
      allowNull: true
    },
    logDate: {
      type: DataTypes.DATE,
      allowNull: true
    },
    logTime: {
      type: DataTypes.DATE,
      allowNull: true
    },
    remarks: {
      type: DataTypes.STRING,
      allowNull: true
    }
  }, {
    tableName: 'trsUserlog'
  });
};
