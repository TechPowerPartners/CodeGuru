import { ChangeDetectionStrategy, Component } from '@angular/core';
import {TuiActiveZoneModule, tuiIsString, TuiPortalModule} from '@taiga-ui/cdk';
import {TuiDataListModule, TuiHostedDropdownModule, TuiSvgModule } from '@taiga-ui/core';
import { TuiTabsModule } from '@taiga-ui/kit';

@Component({
  selector: 'app-app-bar',
  standalone: true,
  imports: [TuiTabsModule,TuiHostedDropdownModule,TuiSvgModule,TuiDataListModule, TuiActiveZoneModule, TuiHostedDropdownModule],
  templateUrl: './app-bar.component.html',
  styleUrl: './app-bar.component.less',
  changeDetection: ChangeDetectionStrategy.OnPush,
  
})
export class AppBarComponent {
  readonly collaborators = ['Carol Cleveland', 'Neil Innes'];
	 
  readonly tabs = [
      'John Cleese',
      'Eric Idle',
      this.collaborators,
      'Michael Palin',
      'Terry Jones',
      'Terry Gilliam',
      'Graham Chapman',
  ];

  activeElement = String(this.tabs[0]);

  get activeItemIndex(): number {
      if (this.collaborators.includes(this.activeElement)) {
          return this.tabs.indexOf(this.collaborators);
      }

      return this.tabs.indexOf(this.activeElement);
  }

  stop(event: Event): void {
      // We need to stop tab custom event so parent component does not think its active
      event.stopPropagation();
  }

  onClick(activeElement: any): void {
      this.activeElement = activeElement;
  }

  isString(tab: unknown): tab is string {
      return tuiIsString(tab);
  }
}