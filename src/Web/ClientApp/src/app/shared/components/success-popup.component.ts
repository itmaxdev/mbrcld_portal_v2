import { Component, Input, Output, EventEmitter } from '@angular/core'

@Component({
  selector: 'app-success-popup',
  template: `
    <div class="popup-overlay" *ngIf="visible" (click)="close()">
      <div
        class="popupBox mdBox"
        style="display: block; position: relative; z-index: 1;"
        (click)="$event.stopPropagation()"
      >
        <div class="inner">
          <div class="thanksPop">
            <div class="iconBox">
              <svg
                xmlns="http://www.w3.org/2000/svg"
                version="1.1"
                xmlns:xlink="http://www.w3.org/1999/xlink"
                width="512"
                height="512"
                x="0"
                y="0"
                viewBox="0 0 52 60"
                style="enable-background: new 0 0 512 512;"
                xml:space="preserve"
              >
                <g>
                  <g fill="currentcolor" fill-rule="nonzero">
                    <path
                      d="M17.206 33.467a9.7 9.7 0 0 1-4.367 1.472A3.993 3.993 0 0 0 9 32H4a4 4 0 0 0-4 4v20a4 4 0 0 0 4 4h5a4 4 0 0 0 3.852-2.98c.85.045 1.681.255 2.45.618.386.183.755.4 1.125.614.552.337 1.127.634 1.721.89 1.548.59 3.195.882 4.852.858h18a4 4 0 0 0 3.314-6.237 3.982 3.982 0 0 0 2-6 3.973 3.973 0 0 0 .72-7.188c.62-.715.963-1.629.966-2.575a4 4 0 0 0-4-4H31.368c1.564-2.978 4.406-9.635 1.64-14.477-1.217-2.127-3.76-3.791-6.461-2.415-2.818 1.432-2.187 4.261-1.883 5.619.775 3.484-1.734 7.098-7.458 10.74zM9 58H4a2 2 0 0 1-2-2V36a2 2 0 0 1 2-2h5a2 2 0 0 1 2 2v20a2 2 0 0 1-2 2zm17.616-35.708c-.452-2.023-.249-2.849.838-3.4 1.764-.9 3.231.6 3.818 1.625 2.989 5.234-2.411 13.861-2.466 13.948A1 1 0 0 0 29.65 36H44a2 2 0 1 1 0 4h-6a1 1 0 0 0 0 2h7a2 2 0 1 1 0 4h-7a1 1 0 0 0 0 2h5a2 2 0 1 1 0 4h-5a1 1 0 0 0 0 2h3a2 2 0 1 1 0 4H23c-1.4.024-2.79-.217-4.1-.712a11.969 11.969 0 0 1-1.469-.766 17.845 17.845 0 0 0-1.267-.689 8.551 8.551 0 0 0-3.164-.8V36.951a11.72 11.72 0 0 0 5.279-1.8c6.51-4.138 9.315-8.465 8.337-12.859zM38.01.971V1c0 .368-.091 9-8.009 9a1 1 0 0 0 0 2c7.887 0 8.008 8.634 8.009 9a1 1 0 0 0 2 .027V21c0-.368.091-9 8.01-9a1 1 0 0 0 0-2c-7.888 0-8.008-8.634-8.01-9a1 1 0 0 0-2-.027zM43.46 11a9.5 9.5 0 0 0-4.451 5.036A9.494 9.494 0 0 0 34.558 11a9.494 9.494 0 0 0 4.451-5.036A9.5 9.5 0 0 0 43.46 11z"
                      opacity="1"
                    ></path>
                    <path
                      d="M13 13v-.006A1 1 0 0 0 11 13c0 .245-.084 6-5 6a1 1 0 0 0 0 2c4.916 0 5 5.757 5 6v.006A1 1 0 0 0 13 27c0-.245.084-6 5-6a1 1 0 0 0 0-2c-4.916 0-5-5.757-5-6zm1.344 7A6.721 6.721 0 0 0 12 22.713 6.721 6.721 0 0 0 9.656 20 6.715 6.715 0 0 0 12 17.287 6.715 6.715 0 0 0 14.344 20zM47 21a1 1 0 0 0-1 1v2a1 1 0 0 0 2 0v-2a1 1 0 0 0-1-1zM47 31a1 1 0 0 0 1-1v-2a1 1 0 0 0-2 0v2a1 1 0 0 0 1 1zM49 27h2a1 1 0 0 0 0-2h-2a1 1 0 0 0 0 2zM45 27a1 1 0 0 0 0-2h-2a1 1 0 0 0 0 2zM21.793 9.207l1.5 1.5a1 1 0 0 0 1.414-1.414l-1.5-1.5a1 1 0 1 0-1.414 1.414zM18.793 6.207a1 1 0 0 0 1.414-1.414l-1.5-1.5a1 1 0 0 0-1.414 1.414zM17.293 10.707a1 1 0 0 0 1.414 0l1.5-1.5a1 1 0 1 0-1.414-1.414l-1.5 1.5a1 1 0 0 0 0 1.414zM22.5 6.5a1 1 0 0 0 .707-.293l1.5-1.5a1 1 0 1 0-1.414-1.414l-1.5 1.5A1 1 0 0 0 22.5 6.5z"
                      opacity="1"
                    ></path>
                  </g>
                </g>
              </svg>
            </div>
            <div class="title lg">{{ title }}</div>
            <div class="textBox">
              <p>{{ message }}</p>
            </div>
            <div class="moreWrap">
              <button type="button" class="more wAuto" (click)="close()">
                <span>Ok</span>
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      .popup-overlay {
        position: fixed;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        background: rgba(0, 0, 0, 0.5);
        z-index: 9999;
        display: flex;
        align-items: center;
        justify-content: center;
      }

      .popup-overlay .popupBox {
        display: block !important;
        border-radius: 20px;
        overflow: hidden;
        background: #fff;
        box-shadow: 0 10px 30px rgba(0, 0, 0, 0.2);
      }

      .thanksPop {
        text-align: center;
        padding: 2rem 1.5rem;
      }

      .thanksPop .iconBox {
        display: flex;
        align-items: center;
        justify-content: center;
        margin: 0 auto 1.5rem;
        width: 80px;
        height: auto;
        color: #555;
      }

      .thanksPop .iconBox svg {
        width: 100%;
        height: auto;
      }

      .thanksPop .title.lg {
        color: #16a34a;
        font-weight: 600;
        font-size: 1.5rem;
        margin-bottom: 0.75rem;
      }

      :host-context(.error-popup) .thanksPop .title.lg {
        color: #dc2626;
      }

      .thanksPop .textBox {
        margin-bottom: 1.5rem;
      }

      .thanksPop .textBox p {
        color: #555;
        font-size: 0.95rem;
        line-height: 1.5;
      }

      .thanksPop .moreWrap {
        display: flex;
        justify-content: center;
      }
    `,
  ],
})
export class SuccessPopupComponent {
  @Input() visible = false
  @Input() title = 'Thank you for registering'
  @Input() message =
    'You will receive a confirmation email shortly with all the details of the event.'

  @Input() type: 'success' | 'error' = 'success'

  @Output() visibleChange = new EventEmitter<boolean>()
  @Output() closed = new EventEmitter<void>()

  close() {
    this.visible = false
    this.visibleChange.emit(false)
    this.closed.emit()
  }
}
