import { Component, OnInit } from '@angular/core'
import { ActivatedRoute } from '@angular/router'
import { Message } from 'primeng/api'
import { ArticlesClient, FileParameter } from 'src/app/shared/api.generated.clients'
import { Location } from '@angular/common'
import { ConfirmationService } from 'primeng/api'
import { tap } from 'rxjs/operators'

@Component({
  selector: 'app-articles-details',
  templateUrl: './articles-details.component.html',
  styleUrls: ['./articles-details.component.scss'],
  providers: [ConfirmationService],
})
export class ArticlesDetailsComponent implements OnInit {
  name = ''
  text: string
  ready = false
  imgUrl1: string
  imgUrl2: string
  caption = $localize`Edit Article`
  onUpdate = false
  createPage = false
  description: string
  msgs: Message[] = []
  imageFile: any = null
  thumbnailFile: any = null
  articleStatus: number
  id: string | undefined
  articleData: any

  constructor(
    private _location: Location,
    private route: ActivatedRoute,
    private article: ArticlesClient,
    private confirmationService: ConfirmationService
  ) {}

  goBack() {
    this._location.back()
  }

  getUploadedFile(event: any) {
    if (event.target.files.length > 0) {
      this.imageFile = event.target.files[0]
    }
  }

  getUploadedThumbnail(event: any) {
    if (event.target.files.length > 0) {
      this.thumbnailFile = event.target.files[0]
    }
  }

  confirm(saveType: number) {
    this.confirmationService.confirm({
      message:
        'The article is already published, upon  saving, it will not be visible for other. Are you sure you want to save?',
      header: 'Confirmation',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.saveArticle(saveType)
      },
      reject: () => {},
    })
  }

  saveArticle(articleStatus: number) {
    this.onUpdate = true
    let orignalFile: FileParameter
    let thumbnailFile: FileParameter
    const dateField = new Date()

    if (this.imageFile) {
      orignalFile = {
        data: this.imageFile,
        fileName: this.imageFile.name,
      }
    } else {
      orignalFile = null
    }
    if (this.thumbnailFile) {
      thumbnailFile = {
        data: this.thumbnailFile,
        fileName: this.thumbnailFile.name,
      }
    } else {
      this.thumbnailFile = null
    }

    return this.article
      .articlesPost(
        orignalFile,
        thumbnailFile,
        this.id,
        this.description,
        this.name,
        this.text,
        articleStatus,
        dateField
      )
      .pipe(
        tap(() => {
          this.onUpdate = false
          this._location.back()
        })
      )
      .toPromise()
  }

  saveAsDraft() {
    this.saveArticle(1)
  }

  sendForApproval() {
    this.saveArticle(2)
  }

  async ngOnInit() {
    this.id = this.route.snapshot.paramMap.get('id')
    if (this.id) {
      await Promise.all([
        this.article.articlesGetById(this.id).subscribe((data) => {
          this.articleData = data
          this.name = this.articleData.name
          this.description = this.articleData.description
          this.imgUrl1 = this.imgUrl2 = this.articleData.pictureUrl
          this.text = this.articleData.theArticle
          this.articleStatus = this.articleData.articleStatus
          this.ready = true
        }),
      ])
    } else {
      this.ready = true
      this.caption = $localize`Add Article`
      this.id = undefined
    }
  }
}
