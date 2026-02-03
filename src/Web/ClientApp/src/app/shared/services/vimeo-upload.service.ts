import { Injectable } from '@angular/core'
import * as tus from 'tus-js-client'
import { Observable } from 'rxjs'
import { HttpHeaders, HttpClient, HttpBackend } from '@angular/common/http'
import { uploadFiles } from '../../home/programs/common/add-video.component'

@Injectable({
  providedIn: 'root',
})
export class VimeoUploadService {
  private http: HttpClient
  private api = 'https://api.vimeo.com/me/videos'
  private accessToken = '8d974707da50354c35c8d4f9a63c716b'
  public isVideoUploaded = false

  constructor(handler: HttpBackend) {
    this.http = new HttpClient(handler)
  }

  createVideo(file: File): Observable<any> {
    this.isVideoUploaded = true
    const body = {
      name: file.name,
      upload: {
        approach: 'tus',
        size: file.size,
      },
    }
    const header: HttpHeaders = new HttpHeaders()
      .set('Authorization', 'bearer ' + this.accessToken)
      .set('Content-Type', 'application/json')
      .set('Accept', 'application/vnd.vimeo.*+json;version=3.4')
    return this.http.post(this.api, body, {
      headers: header,
      observe: 'response',
    })
  }

  public tusUpload(
    file: uploadFiles,
    i: number,
    videoArray: uploadFiles[],
    uploadArray: tus.Upload[],
    success: any
  ): tus.Upload {
    const upload = new tus.Upload(file.video, {
      uploadUrl: file.uploadURI,
      endpoint: file.uploadURI,
      retryDelays: [0],
      onError: (error) => {
        console.log('Failed: ' + file.video.name + error)
      },
      onProgress: (bytesUploaded, bytesTotal) => {
        const percentage = ((bytesUploaded / bytesTotal) * 100).toFixed(2)
      },
      onSuccess: () => {
        if (i < videoArray.length - 1) {
          uploadArray[i + 1].start()
        } else {
          success()
          this.isVideoUploaded = false
        }
      },
    })
    return upload
  }
}
