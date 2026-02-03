import { Component, OnInit, Input } from '@angular/core'

@Component({
  selector: 'app-scholarship-card',
  templateUrl: './scholarship-card.component.html',
  styleUrls: ['./scholarship-card.component.scss'],
})
export class ScholarshipCardComponent implements OnInit {
  @Input()
  title: string

  @Input()
  description: string

  @Input()
  date: Date

  @Input()
  image: string

  ngOnInit(): void {}
}
