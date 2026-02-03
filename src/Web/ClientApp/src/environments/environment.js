"use strict";
// This file can be replaced during build by using the `fileReplacements` array.
// `ng build ---prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.
Object.defineProperty(exports, "__esModule", { value: true });
exports.environment = void 0;
exports.environment = {
    production: false,
    baseUrl: 'https://localhost:5001',
    uaeUrl: 'https://qa-id.uaepass.ae/idshub/authorize',
    logOutUrl: 'https://qa-id.uaepass.ae/idshub/logout',
    clientID: 'mbrcld_web_stage',
    clientSecret: 'Q3JF4VVd6zYVuX8n',
    remoteBackend: false,
};
/*
 * In development mode, to ignore zone related error stack frames such as
 * `zone.run`, `zoneDelegate.invokeTask` for easier debugging, you can
 * import the following file, but please comment it out in production mode
 * because it will have performance impact when throw error
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
// Use to set test.mbrcld.ae as backend
// added by Firas
// export const environment = {
//   production: false,
//   baseUrl: 'http://test.mbrcld.ae', //'https://localhost:5001',
//   uaeUrl: 'https://qa-id.uaepass.ae/idshub/authorize',
//   logOutUrl: 'https://qa-id.uaepass.ae/idshub/logout',
//   clientID: 'mbrcld_web_stage',
//   clientSecret: 'Q3JF4VVd6zYVuX8n',
//   remoteBackend: true,
// }
//# sourceMappingURL=environment.js.map