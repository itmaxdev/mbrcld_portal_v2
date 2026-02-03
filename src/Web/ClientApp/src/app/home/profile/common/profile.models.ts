export interface IUserGeneralInformation {
  salutation?: number
  firstName?: string
  firstName_AR?: string
  middleName?: string
  middleName_AR?: string
  lastName?: string
  lastName_AR?: string
  maritalStatus?: number
  gender?: number
  birthdate?: Date
  nationality?: string
}

export interface IUserContactDetails {
  email?: string
  businessEmail?: string
  mobilePhone?: string
  mobilePhone2?: string
  telephone?: string
  residenceCountry?: string
  city?: string
  postOfficeBox?: string
  address?: string
  linkedInUrl?: string
  twitterUrl?: string
  instagramUrl?: string
}

export interface IUserIdentityDetails {
  emiratesId?: string
  emiratesIdExpiryDate?: Date
  passportNumber?: string
  passportExpiryDate?: Date
  passportIssuingAuthority?: number
}
