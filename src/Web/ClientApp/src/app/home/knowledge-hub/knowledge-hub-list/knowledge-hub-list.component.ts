import { Component, OnInit } from '@angular/core'
import { ScholarshipsClient } from 'src/app/shared/api.generated.clients'
import { ProgramActiveProgramComponent } from '../../programs/programs-registrant/programs-active-program.component'

interface IKnowledgeHubCard {
  imageUrl: string
  groupName: string
  groupId: string
}

@Component({
  selector: 'app-knowledge-hub-list',
  templateUrl: './knowledge-hub-list.component.html',
  styleUrls: ['./knowledge-hub-list.component.scss'],
})
export class KnowledgeHubListComponent implements OnInit {
  knowledgeHubData: IKnowledgeHubCard[] = [
    {
      imageUrl: 'https://cdn.pixabay.com/photo/2017/12/22/08/01/image-3033200_960_720.jpg',
      groupName: 'Technology',
      groupId: 'technology',
    },
    {
      imageUrl: 'https://cdn.pixabay.com/photo/2017/12/22/08/01/image-3033200_960_720.jpg',
      groupName: 'Entrepreneurship',
      groupId: 'entrepreneurship',
    },
    {
      imageUrl:
        'https://www.bodyinnovation.co.za/wp-content/uploads/2019/03/10-SIGNS-YOU-SHOULD-INVEST-IN-YOUR-HEALTH-AND-FITNESS-1080x628.png',
      groupName: 'Health and Fitness',
      groupId: 'health-and-fitness',
    },
    {
      imageUrl:
        'https://static.wixstatic.com/media/995233_205c20182a444b38a0f3b749ed40d23b~mv2.jpg',
      groupName: 'Marketing',
      groupId: 'marketing',
    },
    {
      imageUrl:
        'https://static1.squarespace.com/static/58264005893fc0eadc56471a/5886cafd3e00be4ad2e91f21/5886d104893fc03ef0e58c8c/1485309177826/design+book.jpg?format=1500w',
      groupName: 'Design',
      groupId: 'design',
    },
    {
      imageUrl:
        'https://www.iberdrola.com/wcorp/gc/prod/en_US/comunicacion/superacion_personal_mult_1_res/Superacion_Personal_746x419_EN.jpg',
      groupName: 'Development',
      groupId: 'development',
    },
  ]

  scholarshipsListReady = false
  scholarshipRegistrationListReady = false
  scholarships: Array<any>[]
  scholarshipsRegistrations: Array<any>

  constructor(private client: ScholarshipsClient) {}

  ngOnInit(): void {
    this.fetchScholarshipsList()
  }

  async fetchScholarshipsList() {
    const resp = <any>await this.client.getScholarships()
    this.scholarshipsListReady = true
    if (resp != false) {
      this.scholarships = resp
    }
    console.log(this.scholarships)
  }

  async fetchScholarshipRegistrationList() {
    const resp = <any>await this.client.getScholarshipRegistrations()
    this.scholarshipRegistrationListReady = true
    if (resp != false) {
      for (let x = 0; x < resp.length; x++) {
        if (resp[x].statusCode == 'UnderReview') {
          resp[x].statusCode = 4
        }
        if (resp[x].statusCode == 'Accepted') {
          resp[x].statusCode = 8
        }
        if (resp[x].statusCode == 'Rejected') {
          resp[x].statusCode = 9
        }
      }
      this.scholarshipsRegistrations = resp
    }
    console.log(this.scholarshipsRegistrations)
  }


  handleChangeTab(event) {
    switch (event.index) {
      case 0:
        this.fetchScholarshipsList()
        break
      case 1:
        this.fetchScholarshipRegistrationList()
        break
    }
  }
}
