import { Injectable } from '@angular/core'
import { IReference } from './references.interface'
import { ReferencesClient, AddReferenceCommand } from 'src/app/shared/api.generated.clients'

@Injectable({
  providedIn: 'root',
})
export class ReferencesFacadeService {
  constructor(private referencesClient: ReferencesClient) {}

  loadReferences(): Promise<IReference[]> {
    return this.referencesClient.referencesGet().toPromise()
  }

  addReference(request: IReference): Promise<string> {
    return this.referencesClient.referencesPost(new AddReferenceCommand(request)).toPromise()
  }

  updateReference(id, request: IReference): Promise<void> {
    return this.referencesClient.referencesPut(id, new AddReferenceCommand(request)).toPromise()
  }

  removeReference(id: string): Promise<void> {
    return this.referencesClient.referencesDelete(id).toPromise()
  }
}
