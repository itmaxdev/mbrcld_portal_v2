export interface IProfessionalExperience {
  id?: string
  jobTitle?: string
  jobTitle_AR?: string
  organizationName?: string
  organizationName_AR?: string
  organizationSize?: number
  from?: Date
  to?: Date
  industry?: string
  otherIndustry?: string
  sector?: string
  otherSector?: string
}

export interface ISectorOption {
  value: string
  label: string
}

export interface IIndustryOption {
  value: string
  label: string
}
